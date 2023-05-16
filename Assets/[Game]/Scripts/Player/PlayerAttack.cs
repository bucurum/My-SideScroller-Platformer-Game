using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    [Header("Player Attack")]
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int damageAmount = 1;
    private bool isHit;
    private EnemyHealthController enemyHealthController;
    private Transform tempTransform;
    private PlayerMovementHandler player;
    void Start()
    {
        player = PlayerHealthController.instance.GetComponent<PlayerMovementHandler>();   
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.S) && player.isfallAttacking)
        {
            attackPoint.localPosition = new Vector3(0, -1, 0);
            Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, (attackRange * 1.5f), enemyLayers);
            
            foreach (Collider2D enemy in enemyHit)
            {
                enemy.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
                if (enemy.CompareTag("EnemyWalker"))
                {
                    Debug.Log("general kenobi");
                    player.rb.velocity = new Vector2(player.rb.velocity.x, 20);
                }
            }
            
        }
        
        if (Input.GetMouseButtonDown(0))
        {  
            attackPoint.localPosition = new Vector3(0.706f, 0.073f, 0); 
            Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            
            foreach (Collider2D enemy in enemyHit)
            {
                enemy.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
            }
            
        }
        if (Input.GetMouseButtonUp(1))
        {
            attackPoint.localPosition = new Vector3(0.706f, 0.073f, 0);
            Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
            foreach (Collider2D enemy in enemyHit)
            {
                enemy.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount * 2);
            }
        }
        
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
