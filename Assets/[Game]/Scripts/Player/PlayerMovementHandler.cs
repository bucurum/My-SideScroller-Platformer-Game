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
    [SerializeField] bool canMove;


    
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
    

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbilityTracker>();
        animator = GetComponent<Animator>();
        canMove = true;
    }
    void Update()
    {
        if (canMove)
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
                    transform.localScale = new Vector3(1f,1f,1f);
                }
            }
            
            isGrounded = Physics2D.OverlapCircle(groundPoint.position, .2f, groundLayer);

            if (isGrounded)
            {
                canDoubleJump = true;
            }

            if (Input.GetButtonDown("Jump") && (isGrounded || canDoubleJump))
            {
                if (!isGrounded)
                {
                    canDoubleJump = false;
                    animator.SetTrigger("doubleJump");
                }
                rb.velocity = new Vector2(rb.velocity.y, jumpForce);

            }
            if (Input.GetMouseButtonDown(0))
            {
                animator.SetTrigger("attack");
            }

            if (Input.GetKey(KeyCode.LeftControl))
            {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * crouchSpeed, rb.velocity.y);
                animator.SetBool("crouch", true);
                animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
            }
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, rb.velocity.y);
                animator.SetBool("crouch", false);
                animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));
            }

        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("jump", !isGrounded);
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
