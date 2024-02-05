using UnityEngine;

public class HeadRotation : MonoBehaviour
{
    public Transform headTransform; // Assign the player's head transform in the Inspector

    void Update()
    {
        // Convert the mouse position to a world point
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - transform.position.y));

        // Calculate the direction from the player to the mouse position
        Vector3 direction = (mouseWorldPosition - headTransform.position).normalized;

        // Calculate the angle
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Adjust the angle to be relative to the forward direction of the player
        angle -= headTransform.eulerAngles.y;

        // Normalize the angle to the range [-180, 180]
        angle = NormalizeAngle(angle);

        // Clamp the angle between -38 and 28 degrees
        angle = Mathf.Clamp(angle, -38, 28);

        // Apply the rotation to the player's head
        headTransform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    // Normalize angles to the range [-180, 180]
    private float NormalizeAngle(float angle)
    {
        while (angle > 180) angle -= 360;
        while (angle < -180) angle += 360;
        return angle;
    }
}
