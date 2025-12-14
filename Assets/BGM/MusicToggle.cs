using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    void Start()
    {
        Button btn = GetComponent<Button>();
        
        // 1. Find the background music object
        BackgroundMusic bgm = Object.FindFirstObjectByType<BackgroundMusic>();

        if (bgm != null)
        {
            // 2. Connect the button click to the ToggleMute function
            btn.onClick.AddListener(bgm.ToggleMute);
        }
        else
        {
            Debug.LogWarning("No Background Music found! Did you start from the Login Scene?");
        }
    }
}