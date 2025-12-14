using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance;
    private AudioSource myAudio; // Reference to the speaker

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
            // Get the AudioSource component so we can control it
            myAudio = GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    // Call this function to flip the switch (On -> Off -> On)
    public void ToggleMute()
    {
        if (myAudio != null)
        {
            myAudio.mute = !myAudio.mute; // Invert the current state
        }
    }
}