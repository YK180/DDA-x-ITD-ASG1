using UnityEngine;
using UnityEngine.UI;
using TMPro; // Needed for TextMeshPro Input Fields

public class LoginConnector : MonoBehaviour
{
    // Drag the INPUT FIELDS from your Hierarchy into these slots in the Inspector
    public TMP_InputField currentEmailInput;
    public TMP_InputField currentPasswordInput;
    
    // Drag the FEEDBACK TEXT (if you have one in the scene) here. If not, leave empty.
    public TextMeshProUGUI currentFeedbackText;

    void Start()
    {
        Button myButton = GetComponent<Button>();
        
        // Find the alive Manager
        AuthManager systemManager = Object.FindFirstObjectByType<AuthManager>();

        if (systemManager != null && myButton != null)
        {
            // --- CONNECTING THE WIRES ---
            // We tell the Manager: "Hey, use THESE input fields for this scene!"
            systemManager.emailInput = currentEmailInput; 
            systemManager.passwordInput = currentPasswordInput;
            
            // Connect the feedback text too, so error messages show up
            if (currentFeedbackText != null)
            {
                systemManager.feedbackText = currentFeedbackText;
            }

            // Fix the button click
            myButton.onClick.RemoveAllListeners();
            myButton.onClick.AddListener(systemManager.LoginUser); 
        }
        else
        {
            Debug.LogError("Could not find AuthManager or Button!");
        }
    }
}