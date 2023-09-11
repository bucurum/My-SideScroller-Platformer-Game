using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float arrowSpeed;
    public Rigidbody2D rb;
    public Vector2 moveDirection;
    private PlayerMovementHandler player;
    Weapon weapon;

    PlayerAttack playerAttack;

    void Start()
    {
        player = PlayerHealthController.instance.GetComponent<PlayerMovementHandler>(); 
        weapon = player.GetComponent<PlayerAttack>().weapon;
        playerAttack = player.GetComponent<PlayerAttack>();
        moveDirection = weapon.moveDirection;
        if (player.isFacingRight)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        Debug.Log(playerAttack.holdDownTime);
        
    }

    void Update()
    {  
        
        
        rb.velocity = moveDirection * arrowSpeed; 
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyWalker") || other.CompareTag("EnemyFlyer") || other.CompareTag("Spawner"))
        {
            if (playerAttack.holdDownTime < .5)
            {
                other.GetComponent<EnemyHealthController>().DamageEnemy(weapon.damageAmount);
                Debug.Log("normal damage");
            }
            else
            {
                other.GetComponent<EnemyHealthController>().DamageEnemy(weapon.heavyDamageAmount);
                Debug.Log("heavy");
            }
           
        }
        Destroy(gameObject);
        
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
