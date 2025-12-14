using UnityEngine;

public class ARContentBridge : MonoBehaviour
{
    void Start()
    {
        // Find the GameManager
        GameObject managerObject = GameObject.Find("GameManager"); 
        
        if (managerObject != null)
        {
            BreadGameManager gameManager = managerObject.GetComponent<BreadGameManager>();
            if (gameManager != null)
            {
                // Tell the GameManager THIS object (the ingredients) was spawned
                gameManager.OnIngredientContentSpawned(gameObject);
            }
        }
    }
}