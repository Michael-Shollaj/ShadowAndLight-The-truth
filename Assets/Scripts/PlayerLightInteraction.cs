using UnityEngine;
using UnityEngine.UI;

public class PlayerLightInteraction : MonoBehaviour
{
    public GameObject lightPositionReference; // Reference GameObject for light's position on the player
    private GameObject currentLightObject; // The current light object the player is holding
    private bool hasLight = false; // Flag to check if the player has the light
    private Vector2 lastMoveDir; // To store the last movement direction
    public Text interactionPromptText; // Reference to the interaction prompt UI text

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput;
        }

        bool lightNearby = CheckForNearbyLight();
        interactionPromptText.gameObject.SetActive(lightNearby && !hasLight); // Show text only if there's a light nearby and the player doesn't have a light

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hasLight)
            {
                DropLight();
            }
            else if (lightNearby)
            {
                TryPickupLight();
            }
        }
    }

    private bool CheckForNearbyLight()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Light"))
            {
                return true; // Light is nearby
            }
        }
        return false; // No light nearby
    }

    void TryPickupLight()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Light"))
            {
                currentLightObject = hitCollider.gameObject;
                hasLight = true;
                currentLightObject.transform.SetParent(transform);
                currentLightObject.transform.position = lightPositionReference.transform.position;
                currentLightObject.transform.rotation = lightPositionReference.transform.rotation;
                break; // Pick up the first light found
            }
        }
    }

    void DropLight()
    {
        if (currentLightObject != null)
        {
            currentLightObject.transform.SetParent(null);

            float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg - 90f;
            currentLightObject.transform.rotation = Quaternion.Euler(0, 0, angle);

            Vector3 dropPositionOffset = Quaternion.Euler(0, 0, angle) * Vector3.up * 0.5f;
            currentLightObject.transform.position = transform.position + dropPositionOffset;

            currentLightObject = null;
            hasLight = false;
        }
    }
}
