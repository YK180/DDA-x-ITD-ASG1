using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct BreadData
{
    public string referenceImageName;  // Name in XR Reference Image Library
    public GameObject breadPrefab;     // 3D model prefab
    public string breadTitle;          // Title to display in UI
    [TextArea] public string breadHistory; // History/recipe for UI
}

public class BreadHistoryManager : MonoBehaviour
{
    [Header("AR Components")]
    public ARTrackedImageManager imageManager;

    [Header("Bread Library")]
    public List<BreadData> breadLibrary;

    [Header("UI Elements")]
    public GameObject infoPanel;          // The Panel (Leave ACTIVE in Inspector)
    public TextMeshProUGUI breadNameText; // TMP Text for bread name
    public TextMeshProUGUI breadHistoryText; // TMP Text for bread history
    public Button closeButton;            // Button to close panel

    // Dictionary for quick prefab lookup
    private Dictionary<string, BreadData> _breadDictionary = new Dictionary<string, BreadData>();

    // Keep track of spawned objects to avoid duplicates
    private HashSet<string> _spawnedImageIds = new HashSet<string>();

    // CanvasGroup for smooth show/hide
    private CanvasGroup _panelCanvasGroup;

    private void Awake()
    {
        // 1. Setup UI - Force it active first, then hide via Alpha
        if(infoPanel != null)
        {
            infoPanel.SetActive(true); // Ensure GameObject is ON
            
            _panelCanvasGroup = infoPanel.GetComponent<CanvasGroup>();
            if (_panelCanvasGroup == null)
            {
                _panelCanvasGroup = infoPanel.AddComponent<CanvasGroup>();
            }
            
            // Hide it immediately
            HideInfoPanel();
        }

        // 2. Setup close button listener
        if(closeButton != null)
            closeButton.onClick.AddListener(HideInfoPanel);

        // 3. Setup Dictionary
        foreach (var bread in breadLibrary)
        {
            if (!_breadDictionary.ContainsKey(bread.referenceImageName))
            {
                _breadDictionary.Add(bread.referenceImageName, bread);
            }
        }
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
        // Handle added images
        foreach (var trackedImage in args.added)
        {
            SpawnPrefabForImage(trackedImage);
        }

        // Handle updated images
        foreach (var trackedImage in args.updated)
        {
            string trackableId = trackedImage.trackableId.ToString();

            // Retry spawning if it failed on the first frame (due to missing name)
            if (!_spawnedImageIds.Contains(trackableId) && trackedImage.trackingState == TrackingState.Tracking)
            {
                SpawnPrefabForImage(trackedImage);
            }

            // Handle 3D Object visibility based on tracking
            if(trackedImage.transform.childCount > 0)
            {
                GameObject child = trackedImage.transform.GetChild(0).gameObject;
                bool isVisible = trackedImage.trackingState == TrackingState.Tracking;
                
                if(child.activeSelf != isVisible)
                    child.SetActive(isVisible);
            }
        }
    }

    private void SpawnPrefabForImage(ARTrackedImage trackedImage)
    {
        // SAFETY CHECKS
        if (trackedImage.referenceImage == null) return;

        string imageName = trackedImage.referenceImage.name;
        // If name is empty (happens sometimes in first frame), exit and wait for Update
        if (string.IsNullOrEmpty(imageName)) return;

        string trackableId = trackedImage.trackableId.ToString();
        // If we already spawned for this ID, stop.
        if (_spawnedImageIds.Contains(trackableId)) return;

        // SPAWN LOGIC
        if (_breadDictionary.TryGetValue(imageName, out BreadData breadData))
        {
            if (breadData.breadPrefab != null)
            {
                // Instantiate prefab
                GameObject spawnedObject = Instantiate(breadData.breadPrefab, trackedImage.transform);
                spawnedObject.transform.localPosition = Vector3.zero;
                spawnedObject.transform.localRotation = Quaternion.identity;

                // Mark as spawned
                _spawnedImageIds.Add(trackableId);

                Debug.Log($"Spawned {breadData.breadTitle} for image {imageName}");

                // Show UI
                ShowInfoPanel(breadData.breadTitle, breadData.breadHistory);
            }
        }
    }

    private void ShowInfoPanel(string title, string history)
    {
        if(infoPanel == null || _panelCanvasGroup == null) return;

        // Update Text
        if(breadNameText != null) breadNameText.text = title;
        if(breadHistoryText != null) breadHistoryText.text = history;

        // Force Visuals ON
        infoPanel.SetActive(true); // Just in case
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