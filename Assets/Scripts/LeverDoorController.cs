using UnityEngine;

public class LeverDoorController : MonoBehaviour
{
    public Animator doorAnimator; // Animator for the door
    public Animator leverAnimator; // Animator for the lever

    private bool isDoorOpen = false;

    // Method to be called when the lever is activated
    public void ActivateLever()
    {
        isDoorOpen = !isDoorOpen; // Toggle the door state

        if (isDoorOpen)
        {
            doorAnimator.Play("DoorOpen"); // Play door opening animation
            leverAnimator.Play("LeverDown"); // Play lever down animation
        }
        else
        {
            doorAnimator.Play("DoorClose"); // Play door closing animation
            leverAnimator.Play("LeverUp"); // Play lever up animation
        }
    }
}
