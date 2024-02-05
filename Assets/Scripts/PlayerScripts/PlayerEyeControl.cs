using UnityEngine;

public class PlayerEyeControl : MonoBehaviour
{
    public GameObject blueEye; 
    public GameObject redEye; 

    public FireShooter fireShooter; 
    public LightMaskController lightMaskController;

    private bool isRedEyeActive = false;

    void Start()
    {
        blueEye.SetActive(!isRedEyeActive);
        redEye.SetActive(isRedEyeActive);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchEyes();
        }
    }

    void SwitchEyes()
    {
        isRedEyeActive = !isRedEyeActive;
        Debug.Log("Switching Eyes. Red Eye Active: " + isRedEyeActive);


        if (blueEye != null && redEye != null)
        {
            blueEye.SetActive(!isRedEyeActive);
            redEye.SetActive(isRedEyeActive);
        }
        else
        {
            Debug.LogError("Eye GameObjects not assigned in the inspector!");
        }


        if (fireShooter != null)
        {
            fireShooter.enabled = isRedEyeActive;
        }


        else
        {
            Debug.LogWarning("FireShooter script reference not assigned in the inspector!");
        }


        if (lightMaskController != null)
        {
            lightMaskController.enabled = !isRedEyeActive;
        }
        else
        {
            Debug.LogWarning("LightMaskController script reference not assigned in the inspector!");
        }
    }



}
