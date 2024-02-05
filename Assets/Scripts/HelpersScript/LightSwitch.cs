using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    public GameObject light; // The light object to be turned on
    private GameManager gameManager; // Reference to the GameManager
    private bool isPlayerInRange = false; // Flag to check if player is in range
    private Animator playerAnimator; // Reference to the player's animator component
    private Collider2D collider2D; // Reference to the LightSwitch's collider component
    public GameObject LightoffSprite;
    public GameObject LightonSprite;
    public GameObject InteractButton;
    public GameObject Backgroundoff;
    public GameObject Backgroundon;

    public GameObject happySprite;

    private int enemiesInLight = 0; // Counter for enemies in the light area

    void Start()
    {
        Backgroundoff.SetActive(true);
        LightoffSprite.SetActive(true);
        LightonSprite.SetActive(false);
        InteractButton.SetActive(false);
        happySprite.SetActive(false); // Initially happy sprite is inactive

        // Find the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();
        if (gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }

        // Initially, the light is turned off
        light.SetActive(false);

        // Get the Collider2D component attached to this LightSwitch
        collider2D = GetComponent<Collider2D>();
        if (collider2D == null)
        {
            Debug.LogError("Collider2D not found on the LightSwitch.");
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E) && !light.activeSelf)
        {
            // Turn on the light
            light.SetActive(true);
            LightoffSprite.SetActive(false);
            LightonSprite.SetActive(true);
            Backgroundoff.SetActive(false);
            Backgroundon.SetActive(true);

            if (gameManager != null)
            {
                gameManager.LightTurnedOn(transform.position);
            }

            if (playerAnimator != null)
            {
                playerAnimator.SetTrigger("TurnOnLight");
            }
            else
            {
                Debug.LogError("Player Animator not set.");
            }

            if (collider2D != null)
            {
                collider2D.enabled = false;
            }

            // Check if enemies are present in the light
            UpdateHappySpriteState();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            InteractButton.SetActive(true);
            // Attempt to assign player's Animator
            playerAnimator = other.GetComponent<Animator>();
            if (playerAnimator == null)
            {
                Debug.LogError("Animator component not found on the player.");
            }
        }
        else if (other.CompareTag("EvilShadow") || other.CompareTag("GoodShadow"))
        {
            enemiesInLight++; // Increment counter for enemies in the trigger
            UpdateHappySpriteState(); // Update the happy sprite state when enemies enter
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InteractButton.SetActive(false);
            isPlayerInRange = false;
            playerAnimator = null; // Clear player's Animator reference
        }
        else if (other.CompareTag("EvilShadow") || other.CompareTag("GoodShadow"))
        {
            enemiesInLight--; // Decrement counter for enemies leaving the trigger
            UpdateHappySpriteState(); // Update the happy sprite state when enemies exit
        }
    }

    void UpdateHappySpriteState()
    {
        // Activate happy sprite only if no enemies are in the trigger
        happySprite.SetActive(enemiesInLight == 0);
    }
}
