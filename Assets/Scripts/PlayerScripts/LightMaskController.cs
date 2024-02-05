using UnityEngine;
using UnityEngine.Rendering.Universal; // Required for Light2D

public class LightMaskController : MonoBehaviour
{
    public Light2D myLight; // Assign your 2D light here
    public SpriteMask myMask; // Assign your sprite mask here

    private Vector3 initialMaskSize; // To store the initial size of the mask
    private float initialLightIntensity; // To store the initial intensity of the light
    private float initialLightOuterRadius; // To store the initial outer radius of the light
    private float initialLightInnerRadius; // To store the initial inner radius of the light

    void Start()
    {
        // Store the initial sizes and radii
        initialMaskSize = myMask.transform.localScale;
        initialLightIntensity = myLight.intensity;
        initialLightOuterRadius = myLight.pointLightOuterRadius;
        initialLightInnerRadius = myLight.pointLightInnerRadius;
    }

    void Update()
    {
        if (myLight != null && myMask != null)
        {
            // Calculate the current scale factor based on the light's intensity
            float scaleFactor = myLight.intensity / initialLightIntensity;

            // Apply the scale factor to the mask
            myMask.transform.localScale = initialMaskSize * scaleFactor;

            // Scale the light's outer radius
            myLight.pointLightOuterRadius = initialLightOuterRadius * scaleFactor;

            // Scale the light's inner radius
            myLight.pointLightInnerRadius = initialLightInnerRadius * scaleFactor;
        }
    }
}
