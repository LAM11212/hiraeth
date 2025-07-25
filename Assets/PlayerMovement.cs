using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    bool isFacingRight = true;
    public Rigidbody2D rb;
    public float moveSpeed = 5f;
    float horizontalMovement;
    float verticalMovement;

    [Header("Jump Settings")]
    public float jumpPower = 10f;
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
    public float maxFallSpeed = 18f;
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
    public float wallJumpTime = 0.3f;
    private float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(4f, 7.3f);

    [Header("DashMovement")]
    private bool isDashing;
    private int dashCount = 1;
    private bool canDash;
    public float dashTime = 0.2f;
    public float dashDistance = 5f;
    public float dashPower => dashDistance / dashTime;

    private float dashTimer;
    public Vector2 dashDir;

    //the dash is still a little messed up, y value launches farther due to dashDir.y being 1f as opposed to a diagonal which caps at 0.7f. need to fix this eventually.

    void Update()
    {
        Debug.Log(isDashing);
        GroundCheck();
        Gravity();
        ProcessWallSlide();
        ProcessWallJump();
        ProcessDash();

        if (!isWallJumping && !isDashing)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
            Flip();
        }
    }

    private void Gravity()
    {
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
        horizontalMovement = ctx.ReadValue<Vector2>().x;
        verticalMovement = ctx.ReadValue<Vector2>().y;
    }

    public void Jump(InputAction.CallbackContext ctx)
    {

        if (ctx.performed && jumpsRemaining > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            jumpsRemaining--;
            justJumped = true;
        }
        else if (ctx.canceled && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
        }

        if (ctx.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
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
        }
    }

    public void Dash(InputAction.CallbackContext ctx)
    {
        if(ctx.performed && CanDash())
        {

            dashDir = GetDashDirection();
            isDashing = true;
            dashTimer = 0f;
            dashCount--;
        }
    }

    private void GroundCheck()
    {
        bool grounded = Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer);

        if(grounded && !justJumped)
        {
            jumpsRemaining = 1;
            dashCount = 1;
            isGrounded = true;
        }
        
        if(!grounded && rb.linearVelocity.y <= 0f)
        {
            justJumped = false;
            isGrounded = false;
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
        if(!isGrounded && WallCheck() && horizontalMovement != 0)
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
        if(isDashing && dashTimer < dashTime)
        {
            Debug.Log("dashTimer: " + dashTimer + " dashTime: " + dashTime);
            dashTimer += Time.deltaTime;
            rb.linearVelocity = dashDir * dashPower;
        }
        else if(dashTimer >= dashTime)
        {
            Debug.Log("This actually does go through");
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
        dashTimer = 0f;
        dashDir = Vector2.zero;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }

    private bool CanDash()
    {
        // if dashCount > 0 && !isDashing then the player can dash
        if (dashCount > 0 && !isDashing)
        {
            canDash = true;
            return true;
        }
        canDash = false;
        return false;
    }

    private Vector2 GetDashDirection()
    {
        float x = horizontalMovement;
        float y = verticalMovement;

        Vector2 input = new Vector2(x, y);

        if(input == Vector2.zero)
        {
            return new Vector2(isFacingRight ? 1f : -1f, 0f).normalized;
        }

        return input.normalized;
    }
}
