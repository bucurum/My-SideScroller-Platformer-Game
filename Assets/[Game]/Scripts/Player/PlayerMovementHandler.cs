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
    public Animator animator;
    public bool canMove;
    private bool isFacingRight;
    private float fallAttackSpeed = 50;
    [HideInInspector] public bool isfallAttacking;

    [Header("WallSlide")]
    private bool isWalled;
    private bool isWallSliding;
    [SerializeField] float wallSlidingSpeed = 1f;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;


    [Header("WallJumping")]
    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = .2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = .4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    
    [Header("Jump")]
    public float jumpForce = 1f;
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

    [Header("SwingMechanic")]
    [SerializeField] private DistanceJoint2D distanceJoint2D;
    [SerializeField] private LayerMask anchorLayer;
    private float jointRange = 7f;
    Vector2 prevVelocity;
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] Transform anchorTransform;
    private bool isConnectedAnchor;
    
    //[Header("LedgeClimb")]
    // private bool rayBox1;
    // private bool rayBox2;
    // public float box1OffsetX, box1OffsetY, box1SizeX, box1SizeY, box2OffsetX, box2OffsetY, box2SizeX, box2SizeY;
    // private bool isGrabbing = false;
    // private bool isClimbing = false;
    // private float startingGravity;
    // public LayerMask ledgeLayer;

    [Header("LedgeInfo")]
    [SerializeField] private Vector2 offset1;
    [SerializeField] private Vector2 offset2;

    private Vector2 climbBegunPosition;
    private Vector2 climbOverPosition;

    private bool canGrabLedge = true;
    private bool canClimb;

    [HideInInspector] public bool ledgeDetected;

    private float horizontal;

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbilityTracker>();
        animator = GetComponent<Animator>();
        canMove = true;
        distanceJoint2D.enabled = false;
        // startingGravity = rb.gravityScale;
        
    }
    void Update()
    {
        if (canMove)
        {
            Dash();
            Jump();
            Attack();
            Crouch();
            WallSlide();
            WallJumping();
            Swinging();
            LedgeClimb();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetBool("jump", !isGrounded);
        animator.SetBool("canClimb",canClimb);
       // animator.SetBool("canGrabLedge", !canGrabLedge);
    }

    private void LedgeClimb()
    {
        if (ledgeDetected && canGrabLedge)
        {
            canGrabLedge = false;

            Vector2 ledgePosition = GetComponentInChildren<LedgeDetection>().transform.position;

            climbBegunPosition = ledgePosition + offset1;
            climbOverPosition = ledgePosition + offset2;

            // if (Input.GetKeyDown(KeyCode.W))
            // {
            //     canClimb = true;
            // }
            canClimb = true;
        }
        if (canClimb)
        {
            transform.position = climbBegunPosition;
        }
    }



    // private void ClimbLedge()
    // {
    //     rayBox1 = Physics2D.OverlapBox(new Vector2(transform.position.x + (box1OffsetX * transform.localScale.x), transform.position.y + box1OffsetY), new Vector2(box1SizeX, box1SizeY), 0f ,ledgeLayer);
    //     rayBox2 = Physics2D.OverlapBox(new Vector2(transform.position.x + (box2OffsetX * transform.localScale.x), transform.position.y + box2OffsetY), new Vector2(box2SizeX, box2SizeY), 0f ,ledgeLayer);

    //     if (rayBox2 && !rayBox1 && !isGrabbing)
    //     {
    //         isGrabbing = true;  
    //     }

    //     if (isGrabbing)
    //     {
    //         animator.SetTrigger("isGrabbing");
    //         rb.velocity = Vector2.zero;
    //         rb.gravityScale = 0f;

    //         if (Input.GetKeyDown(KeyCode.W))
    //         {
    //             Debug.Log("Hello there");
    //             isClimbing = true;
    //             StartCoroutine(WaitForAnimation("isClimbing" ,isClimbing));
    //             rb.gravityScale = startingGravity;
    //             isGrabbing = false;
    //           //  StartCoroutine(Climbing(new Vector2(transform.position.x + (1.2f * transform.localScale.x), transform.position.y + 1.7f), .5f));    
    //         } 

    //         if (Input.GetKeyDown(KeyCode.S))
    //         {
    //             rb.gravityScale = startingGravity;
    //             isGrabbing = false;
    //         }   
    //        // animator.SetBool("isClimbing", isClimbing);   
    //     }

    //     Debug.Log(isClimbing);

    // }
    // private IEnumerator WaitForAnimation(string animationName,bool animationBool)
    // {
    //     Debug.Log("genral kenobi");
    //     animator.SetBool("isClimbing", true);
    //     yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
    //     animator.SetBool("isClimbing", false);
    // }
    // private IEnumerator Climbing(Vector2 position, float duration)
    // {
    //     float time = 0;
    //     Vector2 startValue = transform.position;
    //     while (time < duration)
    //     { 
    //         transform.position = Vector2.Lerp(transform.position, position, time / duration);
    //         time += Time.deltaTime;
    //         yield return null;
    //     }
    //     rb.gravityScale = startingGravity;
    //     isGrabbing = false;
    // }
    private void Swinging()
    {
        isConnectedAnchor = Physics2D.OverlapCircle(transform.position, jointRange, anchorLayer);
        if (Input.GetMouseButton(1) && isConnectedAnchor)
        {
            prevVelocity = rb.velocity;
            rb.velocity = prevVelocity * new Vector2(2f, 1f);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, anchorTransform.position);
            lineRenderer.SetPosition(1, transform.position);
            lineRenderer.enabled = true;
            distanceJoint2D.enabled = true;
        }
        if (Input.GetMouseButtonUp(1))
        {
            distanceJoint2D.enabled = false;
            lineRenderer.enabled = false;  

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
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.S) && !isGrounded)
        {
            isfallAttacking = true;
            animator.SetTrigger("fallAttack");
        }
        else
        {
            isfallAttacking = false;
        }
        
        if (Input.GetMouseButton(1) && !isConnectedAnchor)
        {
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * crouchSpeed, rb.velocity.y);
            animator.SetTrigger("heavyAttack");
        }
        if (Input.GetMouseButtonUp(1) && !isConnectedAnchor)
        {
            animator.SetBool("heavyRelease", true);
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
                isFacingRight = false;
            }
            else if (rb.velocity.x > 0)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                isFacingRight = true;
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
        }
        animator.SetBool("wallSlide",isWallSliding);
    }

    private void WallJumping()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
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

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireCube(new Vector2(transform.position.x + (box1OffsetX * transform.localScale.x), transform.position.y + box1OffsetY), new Vector2(box1SizeX, box1SizeY));
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireCube(new Vector2(transform.position.x + (box2OffsetX * transform.localScale.x), transform.position.y + box2OffsetY), new Vector2(box2SizeX, box2SizeY));
    // }
}
