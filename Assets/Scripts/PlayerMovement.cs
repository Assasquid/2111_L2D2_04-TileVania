using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 2f;
    [SerializeField] Vector2 deathKick = new Vector2(10f, 10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform spiritCanon;
    
    Vector2 moveInput;
    Rigidbody2D playerRb;
    Animator playerAnimator; 
    CapsuleCollider2D playerBodyCollider;
    BoxCollider2D playerFeetCollider;
    float normalPlayerGravityScale;
    bool isAlive = true;

    void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerBodyCollider = GetComponent<CapsuleCollider2D>();
        playerFeetCollider = GetComponent<BoxCollider2D>();
        normalPlayerGravityScale = playerRb.gravityScale;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(!isAlive) { return; }

        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnFire(InputValue value)
    {
        if(!isAlive) { return; }
        Instantiate(bullet, spiritCanon.position, transform.rotation);
    }
    

    void OnMove(InputValue value)
    {
        if(!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if(!isAlive) { return; }
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        
        if(value.isPressed)
        {
            playerRb.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRb.velocity.y);
        playerRb.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;
        playerAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(playerRb.velocity.x) > Mathf.Epsilon;

        if(playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRb.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if(!playerFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            playerRb.gravityScale = normalPlayerGravityScale;
            playerAnimator.SetBool("isClimbing", false);
            return;
        }

        playerRb.gravityScale = 0;
        Vector2 climbVelocity = new Vector2(playerRb.velocity.x, moveInput.y * climbSpeed);
        playerRb.velocity = climbVelocity;

        bool playerHasVerticalSpeed = Mathf.Abs(playerRb.velocity.y) > Mathf.Epsilon;
        playerAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        if(playerBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            playerAnimator.SetTrigger("Dying");
            playerRb.velocity = deathKick;
        }
    }
}
