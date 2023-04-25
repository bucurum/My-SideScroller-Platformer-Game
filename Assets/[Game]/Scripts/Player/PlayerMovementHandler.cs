using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Player")]
    public Rigidbody2D rb;
    [SerializeField] float movementSpeed = 1f;
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
    private bool isDashing;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCooldown;
    private float dashTimeLeft;
    private float lastImageXPos;
    private float lastDash = -100f;
    

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbilityTracker>();
        animator = GetComponentInChildren<Animator>();
        canMove = true;
    }
    void Update()
    {
        if (canMove)
        {
            
            if (Input.GetButtonDown("Fire2"))
            {
                if (Time.deltaTime >= lastDash + dashCooldown)
                {
                    AttempToDash();
                }
            }
            
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
            
            isGrounded = Physics2D.OverlapCircle(groundPoint.position, .1f, groundLayer);

            if (isGrounded)
            {
                canDoubleJump = true;
                jumping = false;
            }

            if (Input.GetButtonDown("Jump") && (isGrounded || (canDoubleJump) && abilities.canDoubleJump))
            {
                if (!isGrounded)
                {
                    canDoubleJump = false;
                }
                rb.velocity = new Vector2(rb.velocity.y, jumpForce);
                jumping = true;

            }
            if (Input.GetButtonDown("Fire1"))
            {
                animator.SetTrigger("attack");
            }

            animator.SetBool("jump", jumping);       
        }
        CheckDash();
    }

    private void AttempToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXPos = transform.position.x;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if (dashTimeLeft > 0)
            {
                canMove= false;
                rb.velocity = new Vector2(dashSpeed * transform.localScale.x , rb.velocity.y);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXPos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXPos = transform.position.x;
                }
            }
            if (dashTimeLeft <= 0)
            {
                canMove = true;
                isDashing = false;
            }
        }


        
    }
}
