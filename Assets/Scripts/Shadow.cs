using UnityEngine;

public class Shadow : MonoBehaviour
{
    private CountdownTimer countdownTimer;

    void Start()
    {
        // Find the CountdownTimer script in the scene
        countdownTimer = FindObjectOfType<CountdownTimer>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player")) // Make sure your player GameObject has the tag "Player"
        {
            // Optionally, deactivate or destroy the shadow object if it's a one-time boost
            gameObject.SetActive(false); // Deactivate the shadow object
        }
    }
}
