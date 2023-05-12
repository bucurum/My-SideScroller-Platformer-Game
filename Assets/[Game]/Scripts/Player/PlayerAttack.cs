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


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
            foreach (Collider2D enemy in enemyHit)
            {
                enemy.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
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
