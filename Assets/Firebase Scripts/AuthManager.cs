using System;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 

public class AuthManager : MonoBehaviour
{
    public static AuthManager Instance;
    private FirebaseAuth auth;
    public FirebaseUser User { get; private set; }

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
        var depStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (depStatus != DependencyStatus.Available)
        {
            Debug.LogError("Could not resolve Firebase dependencies: " + depStatus);
            feedbackText.text = "Firebase not available";
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
            bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && User != null)
            {
                Debug.Log("Signed out: " + User.UserId);
            }

            User = auth.CurrentUser;

            if (signedIn)
            {
                Debug.Log("Signed in: " + User.UserId);
                feedbackText.text = "Signed in: " + User.Email;
            }
        }
    }

    public async void RegisterUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        try
        {
            var registerResult = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            User = registerResult.User;
            feedbackText.text = "Registration successful: " + User.Email;
            Debug.Log("User registered: " + User.UserId);

            // Create a record in Firebase DB
            await FirebaseDBManager.Instance.CreateUserRecord(User.UserId, email);
        }
        catch (Exception e)
        {
            Debug.LogError("Registration failed: " + e.Message);
            feedbackText.text = "Registration failed: " + e.Message;
        }
    }

    public async void LoginUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        try
        {
            var loginResult = await auth.SignInWithEmailAndPasswordAsync(email, password);
            User = loginResult.User;
            feedbackText.text = "Login successful: " + User.Email;
            Debug.Log("User logged in: " + User.UserId);

            // Optional: increment login count
            await FirebaseDBManager.Instance.IncrementCounter(User.UserId, "loginCount");
        }
        catch (Exception e)
        {
            Debug.LogError("Login failed: " + e.Message);
            feedbackText.text = "Login failed: " + e.Message;
        }
    }

    private void OnDestroy()
    {
        if (auth != null)
            auth.StateChanged -= AuthStateChanged;
    }
}
