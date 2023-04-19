using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    [Header("Player")]
    public Rigidbody2D rb;
    [SerializeField] float movementSpeed = 1f;

    
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
    }
    void Update()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * movementSpeed, rb.velocity.y);

        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = Vector3.one;
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
        }

    }
}
