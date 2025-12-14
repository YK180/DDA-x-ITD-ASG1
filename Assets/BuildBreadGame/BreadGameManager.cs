using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BreadGameManager : MonoBehaviour
{
    // ... (UI References are fine)
    [Header("UI References")]
    public TextMeshProUGUI instructionsText; 
    public GameObject winPanel;              
    public TextMeshProUGUI winText;          
    public Button bakeButton;                

    [Header("AR References")]
    // NEW: Reference for the finished product
    public GameObject completedBreadPrefab;  

    [Header("Game Settings")]
    public int rewardCoins = 50;             
    
    private bool gameWon = false;
    private string currentUserId;
    
    // NEW: Variable to store the AR_Bread_Content parent when it spawns
    private GameObject currentBreadContent; 

    void Start()
    {
        winPanel.SetActive(false);
        instructionsText.text = "Scan the Image! Tap 'Bake' when ingredients appear.";
        
        if (AuthManager.Instance != null && AuthManager.Instance.User != null)
        {
            currentUserId = AuthManager.Instance.User.UserId;
        }
        else
        {
            Debug.LogError("User not logged in or AuthManager not found!");
            instructionsText.text = "ERROR: Please log in first.";
        }
    }

    // NEW: Call this from the AR Tracked Image Manager's event (see setup below)
    public void OnIngredientContentSpawned(GameObject spawnedObject)
    {
        // Store the reference to the active ingredient content so we can destroy it later
        currentBreadContent = spawnedObject;
        instructionsText.text = "Ingredients Ready! Tap BAKED to finish.";
        // Optionally activate the bake button here if it was disabled before tracking
        if (bakeButton != null) bakeButton.interactable = true;
    }

    public void BakeButtonPress()
    {
        if (gameWon) return; 

        if (bakeButton != null)
        {
            bakeButton.interactable = false;
        }

        instructionsText.text = "";
        
        EndGame();
    }


    async void EndGame()
    {
        gameWon = true;
        
        // 1. DISSAPEAR INGREDIENTS
        if (currentBreadContent != null)
        {
            // Instead of destroying the root object, we hide it for smooth transition
            currentBreadContent.SetActive(false); 
            // NOTE: If you must destroy it, use Destroy(currentBreadContent);
        }

        // 2. SHOW COMPLETED PORK FLOSS BUN PREFAB
        if (completedBreadPrefab != null && currentBreadContent != null)
        {
            // Spawn the completed bread at the same location as the ingredients
            Instantiate(completedBreadPrefab, currentBreadContent.transform.position, currentBreadContent.transform.rotation);
        }
        
        // 3. SHOW WIN PANEL
        winPanel.SetActive(true);
        winText.text = "Bread Baked!\nEarned " + rewardCoins + " Coins";
        
        // 4. FIREBASE LOGIC
        await FirebaseDBManager.Instance.UpdateCoins(currentUserId, rewardCoins);
        await FirebaseDBManager.Instance.IncrementCounter(currentUserId, "gamePlayCount");

        Debug.Log("Rewards saved to Firebase!");
    }

    public void GoToStore()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Store"); 
    }
}