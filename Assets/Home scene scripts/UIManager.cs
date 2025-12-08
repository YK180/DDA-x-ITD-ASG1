using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
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
