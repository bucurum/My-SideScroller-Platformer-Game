using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEditor.Rendering;
using UnityEngine;

public class FlayerMovementHandler : MonoBehaviour
{
    public float rangeToStartChase;
    private bool isChasing;
    
    public Rigidbody2D rb;

    public float knockBackForce;

    [SerializeField] float moveSpeed;
    [SerializeField] float turnSpeed;

    private Transform player;
    public Animator anim;

    void Start()
    {
        player = PlayerHealthController.instance.transform;
    }

    void Update()
    {
        // if (!isChasing)
        // {
            if (Vector3.Distance(transform.position, player.position) < rangeToStartChase)
            {
                {
                    isChasing = true;

                    anim.SetBool("attack", isChasing);
                }
            }
            if (Vector3.Distance(transform.position, player.position) < 4.5f)
            {
                isChasing = false;
                rb.AddForce(new Vector2(5,5));
            }
            

        //}
        // else
        // {
            
            if (player.gameObject.activeSelf && isChasing)
            {
                // if (Vector3.Distance(transform.position, player.position) < .3f)
                // {
                //     rb.AddForce(new Vector2(2,2));
                // }
                Vector3 direction = transform.position - player.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime); 

                //transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                transform.position += (-transform.right) * moveSpeed * Time.deltaTime;
                
            }
            else if (player.gameObject.activeSelf && !isChasing)
            {
                Vector3 direction = transform.position - player.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(-angle, Vector3.forward);

                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime); 

                //transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                transform.position += (-transform.right) * moveSpeed * Time.deltaTime;
            }
        //}
        
    }
    // void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //     {
            
    //     }
    // }
}
