using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    private Transform posA, posB;
    public Transform [] patrolPoints;
    public int platformSpeed;
    Vector2 targetPos;
    Transform originalParent;


    void Start()
    {
        patrolPoints = gameObject.GetComponentsInChildren<Transform>(); //it gets own transform for [0] 
        posA = patrolPoints[1];
        posB = patrolPoints[2];

        targetPos = posA.position;
        foreach (var point in patrolPoints)
        {
            point.SetParent(null);
        }
        originalParent = transform.parent;
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, posA.position) < .1f)
        {
            targetPos = posB.position;
        }
        if (Vector2.Distance(transform.position, posB.position) < .1f)
        {
            targetPos = posA.position;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetPos, platformSpeed * Time.deltaTime);
    }

    private void SetParent(Transform newParent)
    {
        originalParent = transform.parent;
        transform.parent = newParent;
    }

    private void ResetParent()
    {
        transform.parent = originalParent;
    }

    // void OnTriggerEnter2D(Collider2D other)   //thats for player move with platform but when i activate it, the camera not follow the player
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         other.transform.SetParent(this.transform);
    //     }
    // }

    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player"))
    //     {
    //         other.transform.SetParent(null);
    //     }
    // }
}
