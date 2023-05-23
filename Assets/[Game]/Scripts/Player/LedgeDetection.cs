// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class LedgeDetection : MonoBehaviour
// {
//     [SerializeField] private float radius;
//     [SerializeField] private LayerMask ledgeDetectionLayer;
//     private PlayerMovementHandler player;
//     private bool canDetected;

//     void Start()
//     {
//         player = PlayerHealthController.instance.GetComponent<PlayerMovementHandler>();
//     }

//     void Update()
//     {
//         if (canDetected)
//         {
//             player.ledgeDetected =  Physics2D.OverlapCircle(transform.position, radius, ledgeDetectionLayer);
//         }
//     }

//     void OnTriggerEnter2D(Collider2D other)
//     {
//         if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
//         {
//             canDetected = false;
//         }
//     }

//     void OnTriggerExit2D(Collider2D other)
//     {
//         if(other.gameObject.layer == LayerMask.NameToLayer("Ground"))
//         {
//             canDetected = true;
//         }
//     }

//     void OnDrawGizmos()
//     {
//         Gizmos.DrawWireSphere(transform.position, radius);
//     }
// }
