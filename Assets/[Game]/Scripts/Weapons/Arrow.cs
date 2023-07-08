using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed;
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    public int damageAmount;
    private PlayerMovementHandler player;
    Weapon weapon;


    void Start()
    {
        player = PlayerHealthController.instance.GetComponent<PlayerMovementHandler>(); 
        weapon = player.GetComponent<PlayerAttack>().weapon;
        moveDirection = weapon.moveDirection;
    }

    void Update()
    {  
        rb.velocity = moveDirection * arrowSpeed; 
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWalker") || other.CompareTag("EnemyFlyer") || other.CompareTag("Spawner"))
        {
            other.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
            Debug.Log(other.name);
        }
        Destroy(gameObject);
        
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
