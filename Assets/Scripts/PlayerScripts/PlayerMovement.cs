using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "NextScene";
    [SerializeField] private AudioClip WinFX;
    [SerializeField] private AudioClip[] steps;
    [SerializeField] private AudioClip WallJumpFX;
    [SerializeField] private float stepRate = .5f; // Time between steps
    private float nextStepTime = 2f;

    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float currentspeed = 8f;
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float jumpingPower = 16f;
    [SerializeField] private float highJumpPower = 24f;
    private bool isFacingRight = true;
    private NPC_Controller npc;

    private bool isWallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    [SerializeField] private AudioClip JumpFX;

    [SerializeField] private GameObject dustEffect; // Assign your dust particle effect GameObject in the inspector

    [Header("Interactable")]
    public Key followingKey;

    private bool wasGrounded;

    [SerializeField] private float crouchSpeed = 4f;
    private bool isCrouching = false;

    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private float ceilingCheckRadius = 0.2f;
    private bool isCeilingAbove = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;

    // Wall Climbing Variables
    private bool isWallClimbing;
    [SerializeField] private float wallClimbingSpeed = 3f;
    [SerializeField] private Transform wallClimbCheck;
    [SerializeField] private float wallClimbCheckRadius = 0.2f;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float stamina = 100f;
    [SerializeField] private float staminaDecreasePerSecond = 10f;
    [SerializeField] private float staminaRegenPerSecond = 5f;
    [SerializeField] private float sprintStaminaThreshold = 20f; // Minimum stamina needed to sprint

    [Header("Dash")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float timeDash;
    private float gravity;
    private bool CanDoDash = true;
    private bool CanBeMoved = true;


    private Animator anim;

    private bool isSprinting = false;
    private bool canDoubleJump = false;




    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gravity = rb.gravityScale;
    }



    private void Update()
    {
        if (!inDialogue())
        {
            horizontal = Input.GetAxisRaw("Horizontal");

            isCeilingAbove = Physics2D.OverlapCircle(ceilingCheck.position, ceilingCheckRadius, groundLayer);
            isCrouching = Input.GetKey(KeyCode.S) || isCeilingAbove;

            // Modify the isSprinting check to also require horizontal movement
            isSprinting = Input.GetKey(KeyCode.LeftShift) && Mathf.Abs(horizontal) > 0 && !isCrouching && !isWallSliding;

            anim.SetBool("isWalking", Mathf.Abs(horizontal) > 0.1f);
            anim.SetBool("isSprinting", isSprinting);
            anim.SetBool("isCrouching", isCrouching);
            anim.SetBool("isJumping", !IsGrounded());
            anim.SetBool("isWallSliding", isWallSliding);

            if (Input.GetButtonDown("Jump"))
            {
                JumpLogic();
            }

            if (isSprinting && stamina > 0)
            {
                stamina -= staminaDecreasePerSecond * Time.deltaTime;
                if (stamina < 0)
                {
                    stamina = 0;
                    // Ensure sprinting stops if stamina depletes
                    isSprinting = false;
                }
            }
            else if (!isSprinting && stamina < maxStamina)
            {
                stamina += staminaRegenPerSecond * Time.deltaTime;
                if (stamina > maxStamina)
                {
                    stamina = maxStamina;
                }
            }

            // Adjust the sprinting condition based on stamina and horizontal movement
            isSprinting = isSprinting && stamina > sprintStaminaThreshold && Mathf.Abs(horizontal) > 0;

            WallSlide();
            WallClimb();
            WallJump();

            if (!isWallJumping && !isWallClimbing)
            {
                Flip();
            }

            if (Input.GetKeyDown(KeyCode.R) && CanDoDash)
            {
                StartCoroutine(Dash());
            }
        }
    }


    private IEnumerator Dash()
    {
        CanBeMoved = false;
        CanDoDash = false;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(dashSpeed * transform.localScale.x, 0);
        anim.SetTrigger("Dash");

        yield return new WaitForSeconds(timeDash);

        CanBeMoved = true;
        CanDoDash = true;
        rb.gravityScale = gravity;
    }

    private void FixedUpdate()
    {
        if (!isWallJumping && !isWallClimbing)
        {
            MovementLogic();
            PlayFootstepSounds();
        }

        CheckForLanding();
    }

    private void PlayFootstepSounds()
    {
        // Check if the player is on the ground and moving
        if (IsGrounded() && Mathf.Abs(horizontal) > 0.1f && Time.time >= nextStepTime)
        {
            nextStepTime = Time.time + stepRate;

            // Choose a random footstep sound to play
            AudioClip clip = steps[Random.Range(0, steps.Length)];
            SoundFXManager.instance.PlayRandomSoundFXClip(steps, transform, 1f);
        }
    }


    private void JumpLogic()
    {
        if (IsGrounded())
        {
            float jumpForce = isCrouching ? highJumpPower : jumpingPower;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetTrigger("Jump");
            canDoubleJump = true;
            SoundFXManager.instance.PlaySoundFXClip(JumpFX, transform, 1f);
        }
        else if (canDoubleJump && !isWallJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            anim.SetTrigger("DoubleJump");
            canDoubleJump = false;
            SoundFXManager.instance.PlaySoundFXClip(JumpFX, transform, 1f);
        }
    }





    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void WallSlide()
    {
        if (IsWalled() && !IsGrounded() && !isWallClimbing)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }

        anim.SetBool("isWallSliding", isWallSliding);
    }

    private void WallClimb()
    {
        if (IsNearWall() && Input.GetKey(KeyCode.W)) 
        {
            isWallClimbing = true;
            rb.velocity = new Vector2(rb.velocity.x, Input.GetAxisRaw("Vertical") * wallClimbingSpeed);
        }
        else
        {
            isWallClimbing = false;
        }

        anim.SetBool("isWallClimbing", isWallClimbing);
    }

    private bool IsNearWall()
    {
        return Physics2D.OverlapCircle(wallClimbCheck.position, wallClimbCheckRadius, wallLayer);
    }

    private void WallJump()
    {
        if (isWallSliding && Input.GetButtonDown("Jump"))
        {
            isWallJumping = true;
            wallJumpingDirection = -transform.localScale.x;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            SoundFXManager.instance.PlaySoundFXClip(WallJumpFX, transform, 1f);

            // Flip if necessary
            if (transform.localScale.x != wallJumpingDirection)
            {
                Flip();
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void MovementLogic()
    {
        {
            if (CanBeMoved)
            {
                float currentSpeed = isCrouching ? crouchSpeed : (isSprinting ? sprintSpeed : speed);
                rb.velocity = new Vector2(horizontal * currentSpeed, rb.velocity.y);
                
            }
        }

        if (stamina <= 0)
        {
            currentspeed = speed;
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }


    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }


    private void CheckForLanding()
    {
        bool isGroundedNow = IsGrounded();
        if (isGroundedNow && !wasGrounded && rb.velocity.y <= 0f)
        {
            TriggerDustEffect();
        }
        wasGrounded = isGroundedNow; // Update the wasGrounded for the next frame
    }


    private void TriggerDustEffect()
    {
        // Activate the dust effect GameObject, which should play the particle system on awake
        if (dustEffect != null)
        {
            dustEffect.SetActive(true);
            // Optionally, if you want to turn off the effect after a short time, you can start a coroutine to deactivate it
            StartCoroutine(DeactivateAfterDelay(dustEffect, dustEffect.GetComponent<ParticleSystem>().main.duration));
        }
    }

    private IEnumerator DeactivateAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        effect.SetActive(false);
    }


    private void OnDrawGizmosSelected()
    {
        if (ceilingCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(ceilingCheck.position, ceilingCheckRadius);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Portal")
        {
            StartCoroutine(TeleportRoutine());
        }


        if (collision.gameObject.tag == "NPC")
        {
            npc = collision.gameObject.GetComponent<NPC_Controller>();

            if (Input.GetKey(KeyCode.E))
                npc.ActivateDialogue();
        }

    }

    private IEnumerator TeleportRoutine()
    {
        anim.Play("Teleporting"); // Trigger the teleport animation
        yield return new WaitForSeconds(.5f); // Wait for the animation to finish, adjust time as needed

        SceneManager.LoadScene(nextSceneName); // Load the next scene
        SoundFXManager.instance.PlaySoundFXClip(WinFX, transform, 1f);
    }

    private bool inDialogue()
    {
        if (npc != null)
            return npc.DialogueActive();
        else
            return false;
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        npc = null;
    }

}
