using UnityEngine;

public class PlayerLightInteraction : MonoBehaviour
{
    public GameObject lightPositionReference; // Reference GameObject for light's position on the player
    private GameObject currentLightObject; // The current light object the player is holding
    private bool hasLight = false; // Flag to check if the player has the light
    private Vector2 lastMoveDir; // To store the last movement direction

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput != Vector2.zero)
        {
            lastMoveDir = moveInput;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (hasLight)
            {
                DropLight();
            }
            else
            {
                TryPickupLight();
            }
        }
    }

    void TryPickupLight()
    {
        // Check for nearby light objects
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Light")) // Assuming light objects have the tag "Light"
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

            float angle = Mathf.Atan2(lastMoveDir.y, lastMoveDir.x) * Mathf.Rad2Deg;
            currentLightObject.transform.rotation = Quaternion.Euler(0, 0, angle);
            currentLightObject.transform.position = transform.position + new Vector3(lastMoveDir.x, lastMoveDir.y, 0) * 0.5f;

            currentLightObject = null;
            hasLight = false;
        }
    }
}
