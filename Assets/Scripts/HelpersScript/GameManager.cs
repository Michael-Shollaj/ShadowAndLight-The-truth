using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] lights;
    public GameObject door;
    public int lightsOnCount = 0;

    private Vector3 lastCheckpointPosition;

    public PlayerHealth playerHealth;

    public static GameManager instance;

    public Text soulCountText;
    private int totalSouls = 0;

    public Light2D lanternLight; // Reference to the lantern light
    public float maxIntensity = 1.0f; // Maximum intensity of the lantern
    public float dimRate = 0.01f; // Rate at which the light dims over time
    public float rechargeAmount = 0.2f;

    void Start()
    {
        door.SetActive(false);

        if (lanternLight == null)
        {
            Debug.LogError("Lantern Light not set.");
        }
        StartCoroutine(DimLightOverTime());
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void LightTurnedOn(Vector3 checkpointPosition)
    {
        lightsOnCount++;
        if (lightsOnCount == lights.Length)
        {
            OpenDoor();
        }

        lastCheckpointPosition = checkpointPosition;

        if (lanternLight.intensity < maxIntensity)
        {
            lanternLight.intensity += rechargeAmount;
            if (lanternLight.intensity > maxIntensity)
            {
                lanternLight.intensity = maxIntensity;
            }
        }
    }


    public void RespawnPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.transform.position = lastCheckpointPosition; // Respawn player at last checkpoint
            playerHealth.ResetHealth(); // Reset player's health
        }
    }

    public void AddSoul()
    {
        totalSouls++;
        UpdateSoulCountUI();
    }

    void OpenDoor()
    {
        door.SetActive(true);
    }


    private IEnumerator DimLightOverTime()
    {
        while (true)
        {
            if (lanternLight.intensity > 0)
            {
                lanternLight.intensity -= dimRate * Time.deltaTime;
            }
            else
            {
                // Decrease player health every 2 seconds when in darkness
                if (playerHealth != null)
                {
                    StartCoroutine(DarknessDamageRoutine());
                }
            }
            yield return null;
        }
    }

    private void UpdateSoulCountUI()
    {
        if (soulCountText != null)
        {
            soulCountText.text = "Souls: " + totalSouls;
        }
    }


    private IEnumerator DarknessDamageRoutine()
    {
        // Wait 2 seconds before applying damage
        yield return new WaitForSeconds(3);

        // Check if still in darkness before applying damage
        if (lanternLight.intensity <= 0)
        {
            playerHealth.TakeDamage(1, true); // Pass true for light damage
        }
    }

}
