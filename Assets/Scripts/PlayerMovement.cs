using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f;
    public Rigidbody2D rb;
    public Animator anim; // Animator component reference

    public float interactionRange = 1f; // Range for interaction
    public LayerMask leverLayer; // Layer of the lever GameObject
    public KeyCode interactionKey = KeyCode.E; // Key to interact with
    public Text interactionPromptText; // UI text for interaction prompt

    Vector2 movement;
    Vector2 mousePos;
    private bool isDead = false; // Flag to check if the player is dead

    void Update()
    {
        if (isDead) return; // Stop update if the player is dead

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        UpdateAnimation(isSprinting);

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateInteractionPrompt();

        if (Input.GetKeyDown(interactionKey))
        {
            InteractWithLever();
        }
    }

    void FixedUpdate()
    {
        if (isDead) return; // Stop physics update if the player is dead

        float speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) ? moveSpeed * sprintMultiplier : moveSpeed;
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        Vector2 lookDir = mousePos - rb.position;
        rb.rotation = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
    }

    public void TriggerDeath()
    {
        isDead = true;
        anim.SetBool("IsDead", true); // Trigger the death animation
    }

    private void UpdateAnimation(bool isSprinting)
    {
        if (movement.sqrMagnitude > 0.01f)
        {
            anim.Play(isSprinting ? "Sprint" : "Walking");
        }
        else
        {
            anim.Play("Idle");
        }
    }

    private void UpdateInteractionPrompt()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRange, leverLayer);
        bool leverNearby = System.Array.Exists(hits, hit => hit.CompareTag("Lever"));

        interactionPromptText.gameObject.SetActive(leverNearby);
        if (leverNearby)
        {
            interactionPromptText.text = "Press [E] to interact";
        }
    }

    private void InteractWithLever()
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
                    break;
                }
            }
        }
    }

    IEnumerator PlayLeverAnimation()
    {
        anim.SetBool("MoveLever", true);
        yield return new WaitForSeconds(1.30f);
        anim.SetBool("MoveLever", false);
    }
}
