using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Auth; 

public class UIManager : MonoBehaviour
{
    public void Logout()
    {
        // 1. Sign out from Firebase (The "Master Switch")
        if (FirebaseAuth.DefaultInstance != null)
        {
            FirebaseAuth.DefaultInstance.SignOut();
        }

        // REMOVED THE LINE THAT CAUSED THE ERROR (AuthManager.Instance.User = null)
        // Firebase handles the logout state automatically.

        Debug.Log("User logged out.");

        // 2. Go back to Login Screen
        // Make sure "Account Page" is the exact name of your scene
        SceneManager.LoadScene("AccountPage"); 
    }

    public void OpenBuildBread()
    {
        SceneManager.LoadScene("BuildBreadGame");
    }

    public void OpenHistory()
    {
        SceneManager.LoadScene("BreadHistory");
    }

    public void OpenStore()
    {
        SceneManager.LoadScene("Store");
    }
}