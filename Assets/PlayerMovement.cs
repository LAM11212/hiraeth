using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Movement Settings")]
    public bool isFacingRight = true;
    public Rigidbody2D rb;
    public Animator animator;
    public float moveSpeed = 5f;
    float horizontalMovement;
    float verticalMovement;

    [Header("Player State")]
    public PlayerState currentState = PlayerState.Normal;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHangTimer = 0.1f;
    private float hangTimer = 0f;
    public float jumpPower = 8.2f;
    public float jumpCutMultiplier = 0.3f;
    public int jumpsRemaining = 1;
    
    [Header("GroundCheck")]
    private bool justJumped;
    private bool isGrounded;
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 10f;
    public float fallSpeedMultiplier = 2f;
    

    [Header("WallMovmement")]
    public float wallSlideSpeed = 2f;
    private bool isWallSliding;
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask wallLayer;

    //wall jump
    private bool isWallJumping;
    private float wallJumpDir;
    public float wallJumpTime = 0.4f;
    private float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(4f, 7.3f);

    [Header("DashMovement")]
    private bool isDashing;
    public int dashCount = 1;
    public float dashSpeed = 20f;
    private bool canDash;
    public float dashTime = 0.2f;
    public float dashDistance = 2.57f;
    private float dashTimer;
    public Vector2 dashDir;

    [Header("WallClimbing")]
    private bool isWallClimbing;
    private bool wantsToWallClimb = false;
    private const float wallClimbSpeed = 3f;
    public float wallClimbTimer = 0f;
    private const float wallClimbTime = 3f;
    private float wallClimbCooldownTimer = 0f;
    public float wallClimbCooldown = 10f;

    [Header("WallClimbJump")]
    private bool justWallClimbJumped;
    public int wallClimbJumpsRemaining = 3; 

    [Header("Interaction Logic")]
    private IInteractable currentInteractable;
    public LayerMask interactLayer;

    [Header("Spring Movement")]
    public bool bounceOverride = false;
    private float bounceTime = 0.2f;
    private float bounceTimer = 0f;

    //Fire/water crystal stuff
    public bool isMarkedForDeath = false;

    [Header("Cutscene Movement")]
    public bool autoInteract = false;

    //bug fixes:
    //fix issue with not being able to move quickly in opposite direction to wall jump. (slightly fixed, will come back later.)
    //fix issue with wall climb jump not wall climbing again while holding jump button.
    //working on:
    //MAIN OBJ IS SPRITES/ANIMATIONS

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
             Destroy(gameObject);
        }
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {

        if (currentState != PlayerState.Normal) return;
        if (bounceOverride)
        {
            bounceTimer += Time.deltaTime;
            if (bounceTimer > bounceTime)
            {
                bounceOverride = false;
                bounceTimer = 0f;
            }
            return;
        }

        if (hangTimer > 0f)
        {
            hangTimer -= Time.deltaTime;
        }

        GroundCheck();
        Gravity();
        ProcessWallSlide();
        ProcessWallJump();
        ProcessDash();
        ProcessWallclimb();
        CheckForInteractable();
        AutoInteract();

        if (!isWallJumping && !isDashing && !isWallClimbing)
        {
            Flip();
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        }

        justJumped = false;

        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("magnitude", Mathf.Abs(rb.linearVelocity.x));
        //animator.SetBool("isWallSliding", isWallSliding);
    }

    private void Gravity()
    {
        if (isWallClimbing || isDashing || isWallJumping || justWallClimbJumped) return;

        if(rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    public void Move(InputAction.CallbackContext ctx)
    {
        if (currentState != PlayerState.Normal) return;
        Vector2 input = ctx.ReadValue<Vector2>();
        
        if (input.magnitude > 1f)
        {
            if(isFacingRight && input.x > 0f)
            {
                input.x = 1f;
            }
            else if(isFacingRight && input.x < 0f)
            {
                input.x = -1f;
            }
            else if (!isFacingRight && input.x < 0f)
            {
                input.x = -1f;
            }
            else if (!isFacingRight && input.x > 0f)
            {
                input.x = 1f;
            }
        }

        horizontalMovement = input.x;
        verticalMovement = input.y;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (currentState != PlayerState.Normal) return;
        if (ctx.canceled)
        {
            if (justJumped || justWallClimbJumped || rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
            justJumped = false;
            justWallClimbJumped = false;
            return;
        }

        if (ctx.performed && (jumpsRemaining > 0) || isGrounded)
        {
            rb.gravityScale = baseGravity;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            hangTimer = jumpHangTimer;
            jumpsRemaining--;
            animator.SetTrigger("Jump");
            justJumped = true;
            return;
        }

        if (ctx.performed && wallJumpTimer > 0f && !isGrounded && !isWallClimbing)
        {
            isWallJumping = true;
            rb.gravityScale = baseGravity;
            rb.linearVelocity = new Vector2(wallJumpDir * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0f;
            if (transform.localScale.x != wallJumpDir)
            {
                isFacingRight = !isFacingRight;
                Vector3 scale = transform.localScale;
                scale.x *= -1;
                transform.localScale = scale;
            }

            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f);
            return;
        }

        if(ctx.performed && isWallClimbing && !isGrounded)
        {
            if(wallClimbJumpsRemaining <= 0)
            {
                wallClimbTimer = 0f;
                return;
            }
            isWallClimbing = false;
            rb.gravityScale = baseGravity;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            justWallClimbJumped = true;
            wallClimbJumpsRemaining--;
            return;
        }
    }

    public void Dash(InputAction.CallbackContext ctx)
    {
        if (currentState != PlayerState.Normal) return;
        if (ctx.performed && CanDash())
        {
            Vector2 input = new Vector2(horizontalMovement, verticalMovement);

            if(input == Vector2.zero)
                input = new Vector2(isFacingRight ? 1f : -1f, 0f);

            dashDir = input.normalized;
            isDashing = true;
            animator.SetBool("isDashing", true);
            dashTimer = dashTime;
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero;
            dashCount--;
        }
    }

    public void WallClimb(InputAction.CallbackContext ctx)
    {
        if (ctx.performed) wantsToWallClimb = true;
        else if(ctx.canceled) wantsToWallClimb = false;
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if(ctx.performed)
        {
            TryInteract();
        }
    }

    public void AutoInteract()
    {
        if(autoInteract)
        {
            TryInteract();
        }
    }

    private void GroundCheck()
    {
        bool grounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer);

        if(grounded)
        {

            isGrounded = true;
            hangTimer = jumpHangTimer;
            wallClimbTimer = wallClimbTime;
            wallClimbJumpsRemaining = 3;

            if (!WallCheck())
            {
                wallClimbCooldownTimer = 0f;
            }
            
            if(!isDashing)
            {
                jumpsRemaining = 1;
                dashCount = 1;
            }
        }
        else
        {
            isGrounded = false;
            jumpsRemaining = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }

    private void Flip()
    {
        if(isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0f, wallLayer);
    }

    private void ProcessWallSlide()
    {
        if(isWallClimbing)
        {
            isWallSliding = false;
            return;
        }

        if(!isGrounded && WallCheck() && horizontalMovement != 0 && !isWallClimbing)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -wallSlideSpeed));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void ProcessWallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpDir = -transform.localScale.x;
            wallJumpTimer = wallJumpTime;
            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer -= Time.deltaTime;
        }
    }

    private void ProcessDash()
    {
        if (!isDashing) return;

        if(dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
            rb.linearVelocity = dashDir * dashSpeed;
        }
        else
        {
            CancelDash();
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void CancelDash()
    {
        isDashing = false;
        animator.SetBool("isDashing", false);
        dashTimer = 0f;
        dashDir = Vector2.zero;
        rb.gravityScale = baseGravity;
    }

    private bool CanDash()
    {
        if (dashCount > 0 && !isDashing)
        {
            canDash = true;
            return true;
        }
        canDash = false;
        return false;
    }

    private void ProcessWallclimb()
    {
        if (justWallClimbJumped) return;

        if(wallClimbCooldownTimer > 0f)
        {
            wallClimbCooldownTimer -= Time.deltaTime;
        }

        if(wallClimbCooldownTimer > 0f)
        {
            isWallClimbing = false;
            return;
        }

        if(wantsToWallClimb && WallCheck() && !isGrounded)
        {
            if(!isWallClimbing)
            {
                isWallClimbing = true;
                rb.gravityScale = 0f;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            }

            if (wallClimbTimer > 0f)
            {
                wallClimbTimer -= Time.deltaTime;
                float input = verticalMovement;
                rb.linearVelocity = new Vector2(0f, input * wallClimbSpeed);
            }
            else
            {
                isWallClimbing = false;
                rb.gravityScale = baseGravity;
                wallClimbCooldownTimer = wallClimbCooldown;
            }
        }
        else if(isWallClimbing)
        {
            isWallClimbing = false;
            rb.gravityScale = baseGravity;
            wallClimbTimer = 0f;
        }
    }

    private void CheckForInteractable()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, 1f, interactLayer);
        if(hit != null)
        {
            currentInteractable = hit.GetComponent<IInteractable>();
        }
        else
        {
            currentInteractable = null;
        }
    }

    private void TryInteract()
    {
        if(currentInteractable != null && currentInteractable.CanInteract())
        {
            currentInteractable.Interact();
        }
    }

    public void SetState(PlayerState newState)
    {
        currentState = newState;
        rb.linearVelocity = Vector2.zero;
    }
}
