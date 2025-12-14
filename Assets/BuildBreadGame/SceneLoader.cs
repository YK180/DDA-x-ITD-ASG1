using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Make this public so you can type the scene name in the Inspector
    public string sceneToLoad = "Home"; 

    // This function will be called when the button is pressed
    public void LoadTargetScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}