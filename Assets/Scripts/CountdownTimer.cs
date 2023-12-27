using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    public Text timerText;
    public GameObject gameOverPanel;
    public float timeRemaining = 60; // Countdown time in seconds
    public PlayerMovement playerMovement; // Reference to the PlayerMovement script
    public int IncreaseTime;

    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            timeRemaining = 0;
            gameOverPanel.SetActive(true); // Show the game over panel
            if (playerMovement != null)
            {
                playerMovement.TriggerDeath(); // Trigger player's death
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1; // Reset the time scale
    }

}
