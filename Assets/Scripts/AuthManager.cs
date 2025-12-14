using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using TMPro; 

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;
    private FirebaseAuth auth;
    public FirebaseUser User { get; private set; }

    [Header("UI References")]
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI feedbackText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    async void Start()
    {
        // 1. Check Dependencies again to be safe
        var depStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (depStatus != DependencyStatus.Available)
        {
            Debug.LogError("Auth Manager: Could not resolve dependencies: " + depStatus);
            return;
        }

        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != User)
        {
            User = auth.CurrentUser;
            if (User != null)
            {
                Debug.Log("Signed in: " + User.UserId);
                if(feedbackText) feedbackText.text = "Signed in as: " + User.Email;
            }
        }
    }

    // -----------------------------------------------------------------------
    // THIS IS THE CRITICAL FUNCTION
    // -----------------------------------------------------------------------
    public async void RegisterUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        // Basic validation
        if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please enter email and password";
            return;
        }

        feedbackText.text = "Registering...";

        try
        {
            // 1. Create the Account in Firebase Auth
            var registerResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            User = registerResult.User;
            
            Debug.Log("Auth Success! Created User ID: " + User.UserId);
            feedbackText.text = "Account Created! Setting up DB...";

            // 2. IMPORTANT: Create the Database Entry
            // We wait for this to finish before telling the user they are done
            await FirebaseDBManager.Instance.CreateUserRecord(User.UserId, email);

            feedbackText.text = "Success! Account & Database Ready.";
            
            // Optional: Auto-login or move to next scene here
            // UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
        }
        catch (Exception e)
        {
            Debug.LogError("Registration failed: " + e.Message);
            feedbackText.text = "Error: " + e.Message;
        }
    }

    public async void LoginUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        feedbackText.text = "Logging in...";

        try
        {
            var loginResult = await auth.SignInWithEmailAndPasswordAsync(email, password);
            User = loginResult.User;
            
            Debug.Log("User logged in: " + User.UserId);
            feedbackText.text = "Login Successful!";

            // 3. Update Login Count in Database
            await FirebaseDBManager.Instance.IncrementCounter(User.UserId, "loginCount");

            // Load Scene 2 (Home)
            UnityEngine.SceneManagement.SceneManager.LoadScene("Home");
        }
        catch (Exception e)
        {
            Debug.LogError("Login failed: " + e.Message);
            feedbackText.text = "Login Error: " + e.Message;
        }
    }

    private void OnDestroy()
    {
        if (auth != null) auth.StateChanged -= AuthStateChanged;
    }
}