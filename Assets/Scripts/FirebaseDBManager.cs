using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public class FirebaseDBManager : MonoBehaviour
{
    public static FirebaseDBManager Instance;

    public DatabaseReference DBRoot;
    private FirebaseAuth auth;
    private bool isReady = false; 

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        Debug.Log("Checking Firebase Dependencies...");
        var depStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (depStatus != DependencyStatus.Available)
        {
            Debug.LogError("FATAL: Could not resolve Firebase dependencies: " + depStatus);
            return;
        }

        auth = FirebaseAuth.DefaultInstance;
        // We TRY to set it here, but we won't rely on it perfectly.
        DBRoot = FirebaseDatabase.DefaultInstance.RootReference;
        isReady = true; 
        Debug.Log("SUCCESS: Firebase DB is ready to use!");
    }

    // --- THE MAGIC FIX: A HELPER FUNCTION ---
    // This function forces Unity to find the Database even if Start() hasn't finished yet.
    private DatabaseReference GetDB()
    {
        if (DBRoot != null) return DBRoot;

        try 
        {
            // Force connection attempt
            DBRoot = FirebaseDatabase.DefaultInstance.RootReference;
            return DBRoot;
        }
        catch (Exception e)
        {
            Debug.LogError("CRITICAL: Firebase not reachable yet. " + e.Message);
            return null;
        }
    }

    // -------------------------------------------------------------------------
    // 1. CREATE USER
    // -------------------------------------------------------------------------
    public Task CreateUserRecord(string uid, string email)
    {
        // USE THE FIX: Call GetDB() instead of just checking the variable
        DatabaseReference db = GetDB();
        if (db == null) return Task.FromResult(false);

        var userData = new Dictionary<string, object>()
        {
            { "email", email },
            { "signupDate", DateTime.UtcNow.ToString("o") }, 
            { "loginCount", 0 },
            { "gamePlayCount", 0 },
            { "coins", 0 } 
        };

        return db.Child("Users").Child(uid).UpdateChildrenAsync(userData);
    }

    // -------------------------------------------------------------------------
    // 2. INCREMENT COUNTER
    // -------------------------------------------------------------------------
    public Task IncrementCounter(string uid, string counterKey, long increment = 1)
    {
        DatabaseReference db = GetDB();
        if (db == null) return Task.FromResult(false);

        var counterRef = db.Child("Users").Child(uid).Child(counterKey);
        
        var tcs = new TaskCompletionSource<bool>();
        counterRef.RunTransaction(mutableData => {
            long current = 0;
            if (mutableData.Value != null) 
            {
                try { current = Convert.ToInt64(mutableData.Value); } 
                catch { current = 0; }
            }
            mutableData.Value = current + increment;
            return TransactionResult.Success(mutableData);
        }).ContinueWith(task => { 
            if (task.IsFaulted) tcs.SetException(task.Exception); 
            else tcs.SetResult(true); 
        });
        
        return tcs.Task;
    }

    // -------------------------------------------------------------------------
    // 3. UPDATE COINS
    // -------------------------------------------------------------------------
    public Task UpdateCoins(string uid, int amount)
    {
        DatabaseReference db = GetDB();
        if (db == null) return Task.FromResult(false);
        
        var coinRef = db.Child("Users").Child(uid).Child("coins");
        
        var tcs = new TaskCompletionSource<bool>();
        coinRef.RunTransaction(mutableData => {
            long currentCoins = 0;
            if (mutableData.Value != null) 
            {
                try { currentCoins = Convert.ToInt64(mutableData.Value); } 
                catch { currentCoins = 0; }
            }
            
            long newBalance = currentCoins + amount;
            if (newBalance < 0) newBalance = 0; 
            
            mutableData.Value = newBalance;
            return TransactionResult.Success(mutableData);
        }).ContinueWith(task => { 
            if (task.IsFaulted) tcs.SetException(task.Exception); 
            else tcs.SetResult(true); 
        });
        
        return tcs.Task;
    }

    // -------------------------------------------------------------------------
    // 5. READ DATA
    // -------------------------------------------------------------------------
    public Task<DataSnapshot> ReadUserData(string uid)
    {
        DatabaseReference db = GetDB();
        if (db == null) return Task.FromResult<DataSnapshot>(null);

        return db.Child("Users").Child(uid).GetValueAsync();
    }

    // -------------------------------------------------------------------------
    // 6. LOG PURCHASE
    // -------------------------------------------------------------------------
    public Task LogPurchase(string uid, string itemName, double price)
    {
        DatabaseReference db = GetDB();
        if (db == null) return Task.FromResult(false);
        
        var purchaseData = new Dictionary<string, object>() 
        { 
            { "item", itemName }, 
            { "price", price }, 
            { "date", DateTime.UtcNow.ToString("o") } 
        };
        
        return db.Child("Users").Child(uid).Child("purchases").Push().SetValueAsync(purchaseData);
    }
}