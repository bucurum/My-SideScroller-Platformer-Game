using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed;
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    public int damageAmount;

    void Update()
    {
        rb.velocity = moveDirection * arrowSpeed;        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWalker"))
        {
            other.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }
        Destroy(gameObject);
    }
}
