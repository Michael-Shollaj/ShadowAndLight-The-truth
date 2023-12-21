using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleLight : MonoBehaviour
{
    public Light2D candleLight;
    public float maxIntensity = 1.0f;
    public float extinguishTime = 30.0f;
    public bool isLit = true;
    private bool isInTriggerZone = false; // New flag for trigger zone state
    private float timeElapsed;

    void Update()
    {
        if (isInTriggerZone)
        {
            candleLight.intensity = maxIntensity;
            return;
        }

        if (isLit)
        {
            timeElapsed += Time.deltaTime;
            float intensity = Mathf.Lerp(maxIntensity, 0, timeElapsed / extinguishTime);
            candleLight.intensity = intensity;

            if (timeElapsed >= extinguishTime)
            {
                isLit = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleLight();
        }
    }

    public void TurnOnLight()
    {
        isLit = true;
        isInTriggerZone = true; // Set the flag when the light is turned on in a trigger zone
        timeElapsed = 0;
    }

    public void TurnOffLight()
    {
        isLit = false;
        isInTriggerZone = false; // Clear the flag when the light is turned off
    }

    private void ToggleLight()
    {
        if (isLit)
        {
            TurnOffLight();
        }
        else
        {
            TurnOnLight();
        }
    }

    public void ExitTriggerZone() // New method to handle exiting the trigger zone
    {
        isInTriggerZone = false;
        // Optionally, you can start decreasing the light intensity immediately
        // by setting timeElapsed to some value greater than 0
    }
}
