using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool disableMovement = false;
    public float moveSpeed = 5f;
    public float sprintMulti = 1.2f;
    private float currentMulti = 1f;

    public Rigidbody2D rb;
    public Stamina staminaController;
    public PlayerHealth playerHealth;
    public Animator animator;

    private Vector2 movement;
    private bool isSprinting;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer weaponSpriteRenderer;

    [Header("Dashing")]
    public float dashDistance = 2f;
    public float dashCooldown = 1f;
    public float dashStaminaCost = 20f;
    public float dashSpeed = 10f;
    public ParticleSystem dashParticles;
    public TrailRenderer dashTrail;
    public int dashInvincibilityFrames = 25;
    public LayerMask collisionLayers;
    public Transform dashGhostTrail;
    
    public bool isDashing = false;
    private bool canDash = true;

    private bool flipSprite = false;
    
    // Gets the player health if it is not already assigned
    private void Start() {
        if (!playerHealth) playerHealth = GetComponent<PlayerHealth>();
    }

    // Check for input and update stamina
    void Update() {
        // Check if if sprinting
        isSprinting = Input.GetKey(KeyCode.LeftShift);

        // Check for dash input
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

        if (isSprinting && staminaController.currentStamina > 0 && !staminaController.staminaDisabled)
        {
            currentMulti = sprintMulti;
        }
        else
        {
            currentMulti = 1f;
        }

        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetBool("Moving", movement.magnitude > 0);

        // Changes the sprite based on direction of movement
        spriteRenderer.flipX = flipSprite;
        if (weaponSpriteRenderer)
            weaponSpriteRenderer.flipX = flipSprite;
    }

    // Flips sprite based on direction of movement and also creates the ghost trail every tick when dashing
    void FixedUpdate()
    {   

        if (!isDashing && !disableMovement)
        {
            // Calculates the movement vector
            Vector2 movePos = rb.position + moveSpeed * currentMulti * Time.fixedDeltaTime * movement;

            rb.MovePosition(movePos);
        }

        if (movement.x < 0f)
        {
            flipSprite = true;
        }
        else if (movement.x > 0f)
        {
            flipSprite = false;
        }

        if (isDashing == true)
        {
            Instantiate(dashGhostTrail, transform.position, transform.rotation);
        }
    }

    // Dash/Dodge
    IEnumerator Dash()
    {
        if (staminaController.currentStamina <= 0 || staminaController.staminaDisabled)
        {
            yield break;
        }

        // Set flags and subtract stamina
        canDash = false;
        staminaController.SubtractStamina(dashStaminaCost);
        dashParticles.Play();
        dashTrail.emitting = true;
        isDashing = true;

        // Add I-Frames so you don't take damage
        playerHealth.InitInvincibilityFrames(dashInvincibilityFrames);

        Vector2 dashDirection = movement.normalized;

        float dashDuration = dashDistance / dashSpeed;

        Vector2 dashFinalPos = rb.position + dashDirection * dashDistance;

        // Perform actual dash
        float elapsedTime = 0f;

        while (elapsedTime < dashDuration)
        {
            Vector2 nextPosition = Vector2.Lerp(rb.position, dashFinalPos, elapsedTime / dashDuration);

            RaycastHit2D hit = Physics2D.Linecast(rb.position, nextPosition, collisionLayers);

            if (hit.collider)
            {
                dashFinalPos = hit.point - dashDirection * 0.1f;
                nextPosition = Vector2.Lerp(rb.position, dashFinalPos, elapsedTime / dashDuration);
            }

            rb.MovePosition(nextPosition);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Stop dash and reset flags
        dashParticles.Stop();
        dashTrail.emitting = false;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);

        // Reset ability after cooldown is over
        canDash = true;
    }
}
