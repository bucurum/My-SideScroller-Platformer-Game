using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Player")]
    public Rigidbody2D rb;
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] float crouchSpeed = 1f;
    [SerializeField] Animator animator;
    private bool canMove;

    [Header("WallSlide")]
    private bool isWalled;
    private bool isWallSliding;
    [SerializeField] float wallSlidingSpeed = 1f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    
    [Header("Jump")]

    [SerializeField] float jumpForce = 1f;
    public Transform groundPoint;
    private bool isGrounded;
    public LayerMask groundLayer;
    private bool canDoubleJump;
    private bool jumping;

    [Header("Abilities")]
    public PlayerAbilityTracker abilities;

    [Header("Player_Dash")]
    [SerializeField] float dashSpeed;
    [SerializeField] float dashTime;
    private float dashCounter;
    private float dashRechargeCounter;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer afterImage;
    [SerializeField] float afterImageLifeTime;
    [SerializeField] float timeBetweenAfterImage;
    private float afterImageCounter;
    [SerializeField] Color afterImageColor;
    [SerializeField] float waitAfterDashing;
    

    private float horizontal;

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbilityTracker>();
        animator = GetComponent<Animator>();
        canMove = true;
    }
    void Update()
    {
        Movement();

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("jump", !isGrounded);   
    }

    private void Movement()
    {
        if (canMove)
        {
            Dash();
            Jump();
            Attack();
            Crouch();
            WallSlide();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Crouch()
    {
        if (Input.GetKey(KeyCode.LeftControl))
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * crouchSpeed, rb.velocity.y);
            animator.SetBool("crouch", true);
            animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            animator.SetBool("crouch", false);
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("attack");
        }
    }

    private void Jump()
    {
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, .2f, groundLayer);

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || (canDoubleJump && abilities.canDoubleJump)))
        {
            if (!isGrounded)
            {
                canDoubleJump = false;
                animator.SetTrigger("doubleJump");
            }
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);

        }
    }

    private void Dash()
    {
        if (dashRechargeCounter > 0)
        {
            dashRechargeCounter -= Time.deltaTime;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                dashCounter = dashTime;
                ShowAfterImage();
            }
        }

        if (dashCounter > 0)
        {
            dashCounter -= Time.deltaTime;
            rb.velocity = new Vector2(dashSpeed * transform.localScale.x, rb.velocity.y);

            afterImageCounter -= Time.deltaTime;
            if (afterImageCounter < 0)
            {
                ShowAfterImage();
            }
            dashRechargeCounter = waitAfterDashing;
        }
        else
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, rb.velocity.y);

            animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));

            if (rb.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }

    private void WallSlide()
    {
        isWalled = Physics2D.OverlapCircle(wallCheck.position, .2f, wallLayer);

        if (isWalled && !isGrounded)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.x, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
            Debug.Log("wallsliding false");
        }
        animator.SetBool("wallSlide",isWallSliding);
    }




    private void ShowAfterImage()
    {
        SpriteRenderer image = Instantiate(afterImage, transform.position , Quaternion.identity);
        image.sprite = spriteRenderer.sprite;
        image.transform.localScale = transform.localScale;
        image.color = afterImageColor;

        Destroy(image.gameObject, afterImageLifeTime);

        afterImageCounter = timeBetweenAfterImage;
    }
}
