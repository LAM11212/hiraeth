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
    public float dashPower = 10f;
    public float dashTime = 0.2f;
    private float dashTimer;

    void Update()
    {
        
        GroundCheck();
        Gravity();
        ProcessWallSlide();
        ProcessWallJump();
        ProcessDash();
        

        if (!isWallJumping)
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
            isDashing = true;
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            dashTimer = 0f;
            Invoke(nameof(CancelDash), dashTime + 0.1f);
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
        if(isDashing)
        {
            isDashing = false;
            float dashDirX = horizontalMovement;
            float dashDirY = rb.linearVelocity.y;
            Vector2 dashDir = new Vector2(dashDirX, dashDirY).normalized;
            rb.AddForce(dashDir * dashPower, ForceMode2D.Impulse);
            dashCount--;
            CancelInvoke(nameof(CancelDash));
        }
        else if(dashTimer > 0f)
        {
            dashTimer -= Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void CancelDash()
    {
        isDashing = false;
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
}
