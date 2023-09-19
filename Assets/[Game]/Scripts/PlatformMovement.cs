using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{
    public Transform posA, posB;
    public int platformSpeed;
    Vector2 targetPos;

    void Start()
    {
        targetPos = posA.position;
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
