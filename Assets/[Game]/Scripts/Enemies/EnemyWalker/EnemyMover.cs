using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public Transform[] patrolPoints;
    private int currentPoint;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    public Rigidbody2D rb;
    public Animator animator;
    public bool hit;
    private bool isChasing;
    Transform player;
    public float chaceDistane;
    public float maxDistane;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        foreach(var point in patrolPoints)
        {
            point.SetParent(null);
        }
    }

    void Update()
    {
        if (!hit)
        {
            ChacePlayer();
            if (!isChasing) Patrol();
        }
        else
        {
            StartCoroutine(Bounce());
        }
    }

    private void Patrol()
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

    private void ChacePlayer()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < chaceDistane)
        {
            isChasing = true;
            Debug.Log("sealmiasd");
            //rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            //transform.position += transform.forward * moveSpeed * Time.deltaTime;
            Vector3.MoveTowards(transform.position, player.position, chaceDistane);

        }
        if(Vector3.Distance(transform.position, player.transform.position) >= chaceDistane)
        {
            isChasing = false;
            Debug.Log("bitti");
        }
        
    }
    IEnumerator Bounce()
    {
        rb.AddRelativeForce(new Vector2(3,4));
        yield return new WaitForSeconds(.5f);
        hit = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.gameObject.transform.position, chaceDistane);
    }
}
