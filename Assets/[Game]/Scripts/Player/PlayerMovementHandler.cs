using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Player")]
    public Rigidbody2D rb;
    [SerializeField] float movementSpeed = 1f;
    [SerializeField] Animator animator;

    
    [Header("Jump")]

    [SerializeField] float jumpForce = 1f;
    public Transform groundPoint;
    private bool isGrounded;
    public LayerMask groundLayer;
    private bool canDoubleJump;

    [Header("Abilities")]
    public PlayerAbilityTracker abilities;
    

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        abilities = GetComponent<PlayerAbilityTracker>();
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, rb.velocity.y);

        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));

        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-.5f, .5f, .5f);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(.5f,.5f,.5f);
        }

        isGrounded = Physics2D.OverlapCircle(groundPoint.position, .1f, groundLayer);

        if (isGrounded)
        {
            canDoubleJump = true;
        }

        if (Input.GetButtonDown("Jump") && (isGrounded || (canDoubleJump) && abilities.canDoubleJump))
        {
            if (!isGrounded)
            {
                canDoubleJump = false;
            }
            rb.velocity = new Vector2(rb.velocity.y, jumpForce);
            animator.SetBool("jump", isGrounded);
        }

        
    }
}
