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

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        // Check Firebase dependencies
        var depStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (depStatus != DependencyStatus.Available)
        {
            Debug.LogError("Could not resolve Firebase dependencies: " + depStatus);
            return;
        }

        auth = FirebaseAuth.DefaultInstance;
        DBRoot = FirebaseDatabase.DefaultInstance.RootReference;

        Debug.Log("Firebase DB ready.");
    }

    // -------------------------------------------------------------------------
    // USER CREATION
    // -------------------------------------------------------------------------

    public Task CreateUserRecord(string uid, string email)
    {
        var userData = new Dictionary<string, object>()
        {
            { "email", email },
            { "signupDate", DateTime.UtcNow.ToString("o") },
            { "loginCount", 0 },
            { "gamePlayCount", 0 },
            { "visitedStore", false }
        };

        return DBRoot.Child("Users").Child(uid).UpdateChildrenAsync(userData);
    }

    // -------------------------------------------------------------------------
    // SAFE COUNTER INCREMENT (NEW 2024+ API)
    // -------------------------------------------------------------------------

    public Task IncrementCounter(string uid, string counterKey, long increment = 1)
{
    var counterRef = DBRoot.Child("Users").Child(uid).Child(counterKey);
    var tcs = new TaskCompletionSource<bool>();

    counterRef.RunTransaction(mutableData =>
    {
        long current = 0;
        if (mutableData.Value != null)
        {
            try { current = Convert.ToInt64(mutableData.Value); }
            catch { current = 0; }
        }

        mutableData.Value = current + increment;
        return TransactionResult.Success(mutableData);
    }).ContinueWith(task =>
    {
        if (task.IsFaulted)
            tcs.SetException(task.Exception);
        else
            tcs.SetResult(true);
    });

    return tcs.Task;
}

    // -------------------------------------------------------------------------
    // STORE VISIT FLAG
    // -------------------------------------------------------------------------

    public Task SetVisitedStore(string uid, bool visited = true)
    {
        return DBRoot.Child("Users").Child(uid)
                     .Child("visitedStore")
                     .SetValueAsync(visited);
    }

    // -------------------------------------------------------------------------
    // LOG PURCHASE
    // -------------------------------------------------------------------------

    public Task LogPurchase(string uid, string itemName, double price)
    {
        var purchaseData = new Dictionary<string, object>()
        {
            { "item", itemName },
            { "price", price },
            { "date", DateTime.UtcNow.ToString("o") }
        };

        return DBRoot.Child("Users").Child(uid)
                     .Child("purchases")
                     .Push()
                     .SetValueAsync(purchaseData);
    }

    // -------------------------------------------------------------------------
    // READ USER DATA
    // -------------------------------------------------------------------------

    public Task<DataSnapshot> ReadUserData(string uid)
    {
        return DBRoot.Child("Users").Child(uid).GetValueAsync();
    }
}
