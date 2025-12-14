using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

[System.Serializable]
public struct BreadData
{
    public string referenceImageName;  
    public GameObject breadPrefab;     
    public string breadTitle;          
    [TextArea] public string breadHistory; 
}

public class BreadHistoryManager : MonoBehaviour
{
    [Header("AR Components")]
    public ARTrackedImageManager imageManager;
    [Header("Spawning Settings")]
    public float floatHeight = 0.05f;

    [Header("Bread Library")]
    public List<BreadData> breadLibrary;

    [Header("UI Elements")]
    public GameObject infoPanel;          
    public TextMeshProUGUI breadNameText; 
    public TextMeshProUGUI breadHistoryText; 
    public Button closeButton;            
    public Button backButton;             
    public string homeSceneName = "Home"; 
    
    [Header("Instructions")]
    public GameObject scanInstructionText; 

    // --- 1. NEW PROGRESS & REWARD VARIABLES ---
    [Header("Game Progress")]
    public TextMeshProUGUI progressText;  // Drag a new Text for "Found: 0/3"
    public TextMeshProUGUI rewardText;    // Drag a new Text for "+100 Coins!" (Optional)
    public int totalToFind = 3;           // Set this to how many images you have
    public int rewardAmount = 100;
    
    private HashSet<string> _foundBreadNames = new HashSet<string>(); // Tracks UNIQUE breads
    private bool _rewardClaimed = false;
    private string _currentUserId;
    // ------------------------------------------

    private Dictionary<string, BreadData> _breadDictionary = new Dictionary<string, BreadData>();
    private HashSet<string> _spawnedImageIds = new HashSet<string>();
    private CanvasGroup _panelCanvasGroup;

    private void Awake()
    {
        // Setup UI Panel
        if(infoPanel != null)
        {
            infoPanel.SetActive(true); 
            _panelCanvasGroup = infoPanel.GetComponent<CanvasGroup>();
            if (_panelCanvasGroup == null) _panelCanvasGroup = infoPanel.AddComponent<CanvasGroup>();
            HideInfoPanel();
        }

        if (scanInstructionText != null) scanInstructionText.SetActive(true);
        if (rewardText != null) rewardText.text = ""; // Clear reward text initially

        // Button Listeners
        if(closeButton != null) closeButton.onClick.AddListener(HideInfoPanel);
        if (backButton != null) backButton.onClick.AddListener(GoBackHome);

        // Setup Dictionary
        foreach (var bread in breadLibrary)
        {
            if (!_breadDictionary.ContainsKey(bread.referenceImageName))
            {
                _breadDictionary.Add(bread.referenceImageName, bread);
            }
        }
        
        // --- 2. GET USER ID & INIT UI ---
        if (AuthManager.Instance != null && AuthManager.Instance.User != null)
        {
            _currentUserId = AuthManager.Instance.User.UserId;
        }
        UpdateProgressUI();
        // --------------------------------
    }

    void GoBackHome()
    {
        SceneManager.LoadScene(homeSceneName);
    }

    private void OnEnable()
    {
        if (imageManager != null)
            imageManager.trackablesChanged.AddListener(OnTrackablesChanged);
    }

    private void OnDisable()
    {
        if (imageManager != null)
            imageManager.trackablesChanged.RemoveListener(OnTrackablesChanged);
    }

    private void OnTrackablesChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var trackedImage in args.added) SpawnPrefabForImage(trackedImage);
        foreach (var trackedImage in args.updated)
        {
            string trackableId = trackedImage.trackableId.ToString();
            if (!_spawnedImageIds.Contains(trackableId) && trackedImage.trackingState == TrackingState.Tracking)
            {
                SpawnPrefabForImage(trackedImage);
            }
            if(trackedImage.transform.childCount > 0)
            {
                GameObject child = trackedImage.transform.GetChild(0).gameObject;
                bool isVisible = trackedImage.trackingState == TrackingState.Tracking;
                if(child.activeSelf != isVisible) child.SetActive(isVisible);
            }
        }
    }

    private void SpawnPrefabForImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.referenceImage == null) return;

        string imageName = trackedImage.referenceImage.name;
        if (string.IsNullOrEmpty(imageName)) return;

        string trackableId = trackedImage.trackableId.ToString();
        if (_spawnedImageIds.Contains(trackableId)) return;

        if (_breadDictionary.TryGetValue(imageName, out BreadData breadData))
        {
            if (breadData.breadPrefab != null)
            {
                GameObject spawnedObject = Instantiate(breadData.breadPrefab, trackedImage.transform);
                spawnedObject.transform.localPosition = new Vector3(0, floatHeight, 0);
                spawnedObject.transform.localRotation = Quaternion.identity;

                _spawnedImageIds.Add(trackableId);

                // Hide instructions
                if (scanInstructionText != null) scanInstructionText.SetActive(false);

                // --- 3. CHECK PROGRESS ---
                // We check if we have found this TYPE of bread before
                if (!_foundBreadNames.Contains(imageName))
                {
                    _foundBreadNames.Add(imageName);
                    UpdateProgressUI();
                    CheckForCompletion();
                }
                // -------------------------

                ShowInfoPanel(breadData.breadTitle, breadData.breadHistory);
            }
        }
    }

    // --- 4. NEW HELPER FUNCTIONS ---
    void UpdateProgressUI()
    {
        if (progressText != null)
        {
            progressText.text = $"Found: {_foundBreadNames.Count}/{totalToFind}";
        }
    }

    async void CheckForCompletion()
    {
        if (_foundBreadNames.Count >= totalToFind && !_rewardClaimed)
        {
            _rewardClaimed = true;
            Debug.Log("Collection Complete! Giving Reward.");

            // Show Reward Text
            if (rewardText != null) 
            {
                rewardText.text = "All Found! +" + rewardAmount + " Coins!";
                rewardText.color = Color.yellow; 
            }
            
            // Send to Firebase
            if (FirebaseDBManager.Instance != null && !string.IsNullOrEmpty(_currentUserId))
            {
                await FirebaseDBManager.Instance.UpdateCoins(_currentUserId, rewardAmount);
                await FirebaseDBManager.Instance.IncrementCounter(_currentUserId, "collectionsCompleted");
            }
        }
    }
    // -------------------------------

    private void ShowInfoPanel(string title, string history)
    {
        if(infoPanel == null || _panelCanvasGroup == null) return;
        if(breadNameText != null) breadNameText.text = title;
        if(breadHistoryText != null) breadHistoryText.text = history;

        infoPanel.SetActive(true); 
        _panelCanvasGroup.alpha = 1f;
        _panelCanvasGroup.interactable = true;
        _panelCanvasGroup.blocksRaycasts = true;
    }

    private void HideInfoPanel()
    {
        if(_panelCanvasGroup == null) return;
        _panelCanvasGroup.alpha = 0f;
        _panelCanvasGroup.interactable = false;
        _panelCanvasGroup.blocksRaycasts = false;
    }
}