using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    public Rigidbody2D rb;
    public Animator animator;

    void Start()
    {
        foreach(var point in patrolPoints)
        {
            point.SetParent(null);
        }
    }

    void Update()
    {
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > .2f)
        {
            if (transform.position.x < patrolPoints[currentPoint].position.x)
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                transform.localScale = Vector3.one;
            }

            if (transform.position.y < (patrolPoints[currentPoint].position.y - .5f) && rb.velocity.y < .1f)
            {
                rb.velocity = new Vector2(0f, jumpForce);
            }
        }
        else
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            currentPoint++;
            if (currentPoint >= patrolPoints.Length)
            {
                currentPoint = 0;
            }
        }
        animator.SetFloat("speed", Mathf.Abs(rb.velocity.x));      
    }


}
