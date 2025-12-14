using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks; 
using Firebase.Database; // Required for DataSnapshot

// DATA CLASS
[System.Serializable]
public class StoreItem
{
    public string itemName;
    public int price;
    public string firebaseId; 
    public Sprite itemIcon;
}

public class StoreManager : MonoBehaviour
{
    [Header("UI Display")]
    public TextMeshProUGUI userCoinsText;
    public TextMeshProUGUI feedbackText;

    [Header("Store Catalog")]
    public List<StoreItem> catalog; 

    [Header("References")]
    public GameObject itemPrefab; 
    public Transform contentGrid; 

    private string currentUserId;
    private int currentCoinCount = 0; // NEW: Variable to store actual coin number

    async void Start()
    {
        // SAFETY CHECK 1: Is AuthManager ready?
        if (AuthManager.Instance == null || AuthManager.Instance.User == null)
        {
            Debug.LogError("STOP: User not logged in. Start from Scene 1 (Login)!");
            if(feedbackText) feedbackText.text = "Error: Not Logged In";
            return;
        }

        currentUserId = AuthManager.Instance.User.UserId;
        Debug.Log("Store loaded for user: " + currentUserId);

        // Run the refresh logic
        await RefreshCoins();
        
        // Build the store UI
        PopulateStore();
    }

    // --- REFRESH FUNCTION WITH RETRY & NUMBER PARSING ---
    async Task RefreshCoins()
    {
        if (FirebaseDBManager.Instance == null) return;

        int retries = 0;
        DataSnapshot snapshot = null;

        // RETRY LOOP: Try to get data up to 5 times
        while (retries < 5)
        {
            snapshot = await FirebaseDBManager.Instance.ReadUserData(currentUserId);
            
            if (snapshot != null) 
            {
                break; // Valid data found
            }

            // If null, wait 0.5 seconds and try again
            Debug.LogWarning($"Store: DB not ready. Retrying... ({retries + 1}/5)");
            await Task.Delay(500); 
            retries++;
        }

        // Final Safety Check
        if (snapshot == null)
        {
            Debug.LogError("Store: Could not load coins after 5 tries.");
            userCoinsText.text = "Coins: --";
            return;
        }

        // Update UI & Internal Variable
        if (snapshot.Exists && snapshot.Child("coins").Value != null)
        {
            string coinValue = snapshot.Child("coins").Value.ToString();
            userCoinsText.text = "Coins: " + coinValue;
            
            // --- NEW: Convert string to number for math checks ---
            int.TryParse(coinValue, out currentCoinCount);
            // ----------------------------------------------------
            
            Debug.Log("Store: Coins updated to " + coinValue);
        }
        else
        {
            userCoinsText.text = "Coins: 0";
            currentCoinCount = 0;
        }
    }

    void PopulateStore()
    {
        // Clear old items just in case
        foreach (Transform child in contentGrid) Destroy(child.gameObject);

        foreach (var item in catalog)
        {
            GameObject card = Instantiate(itemPrefab, contentGrid);
            
            // Setup Text
            var nameText = card.transform.Find("ItemName");
            var priceText = card.transform.Find("PriceTag");
            var iconImage = card.transform.Find("Icon");

            if (nameText != null) nameText.GetComponent<TextMeshProUGUI>().text = item.itemName;
            if (priceText != null) priceText.GetComponent<TextMeshProUGUI>().text = item.price + " Coins";
            if (iconImage != null) iconImage.GetComponent<Image>().sprite = item.itemIcon;

            // Find the child object named "BuyButton" and get its button component
            Transform btnObj = card.transform.Find("BuyButton");
            if (btnObj != null)
            {
                btnObj.GetComponent<Button>().onClick.AddListener(() => Purchase(item));
            }

            // This forces the Grid Layout Group to wake up and move the items immediately.
            LayoutRebuilder.ForceRebuildLayoutImmediate(contentGrid.GetComponent<RectTransform>());
        }
    }

    IEnumerator FixLayout()
    {
        yield return new WaitForEndOfFrame();

        var group = contentGrid.GetComponent<GridLayoutGroup>();
        group.enabled = false;
        yield return new WaitForEndOfFrame();
        group.enabled = true;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentGrid.GetComponent<RectTransform>());
    }

    // --- PURCHASE FUNCTION WITH PRICE CHECK ---
    public async void Purchase(StoreItem item)
    {
        feedbackText.text = "Processing...";
        
        // 1. CHECK IF USER HAS ENOUGH MONEY
        // If current coins are less than the item price, stop the purchase.
        if (currentCoinCount < item.price)
        {
            feedbackText.text = "Not enough coins!";
            feedbackText.color = Color.red; // Optional: Make text red
            Debug.LogWarning("Purchase Denied: " + currentCoinCount + " < " + item.price);
            
            // Reset text color after 2 seconds (optional nice-to-have)
            await Task.Delay(2000);
            if(feedbackText) {
                feedbackText.text = "";
                feedbackText.color = Color.white;
            }
            return; // STOP HERE.
        }

        // 2. If we pass the check, reset color and buy
        feedbackText.color = Color.white; 
        feedbackText.text = "Buying " + item.itemName + "...";
        
        // Deduct coins
        await FirebaseDBManager.Instance.UpdateCoins(currentUserId, -item.price);
        // Log purchase
        await FirebaseDBManager.Instance.LogPurchase(currentUserId, item.itemName, item.price);
        
        feedbackText.text = "Bought " + item.itemName + "!";
        
        // Update the UI immediately
        await RefreshCoins();
    }
}