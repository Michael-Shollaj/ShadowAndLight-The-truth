using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public Rigidbody2D rb;
    public Animator anim; // Animator component reference

    public float interactionRange = 1f; // Range within which the player can interact with the lever
    public LayerMask leverLayer; // Layer of the lever GameObject
    public KeyCode interactionKey = KeyCode.E; // Key to interact with

    Vector2 movement;
    Vector2 mousePos;

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Check if the player is moving
        if (movement.sqrMagnitude > 0.01f)
        {
            if (isSprinting)
            {
                anim.Play("Sprint"); // Play sprinting animation
            }
            else
            {
                anim.Play("Walking"); // Play walking animation
            }
        }
        else
        {
            anim.Play("Idle"); // Play idle animation
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown(interactionKey))
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRange, leverLayer);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Lever"))
                {
                    LeverDoorController lever = hit.GetComponent<LeverDoorController>();
                    if (lever != null)
                    {
                        lever.ActivateLever(); // Activate the lever
                        StartCoroutine(PlayLeverAnimation()); // Play interaction animation
                        break; // Assuming only one lever can be activated at a time
                    }
                }
            }
        }
    }

    IEnumerator PlayLeverAnimation()
    {
        anim.SetBool("MoveLever", true);
        yield return new WaitForSeconds(1.30f); // Adjust duration to match the animation length
        anim.SetBool("MoveLever", false);
    }

    void FixedUpdate()
    {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        float speed = isSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
}
