using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Attack")]
    public Weapon weapon;
    private Transform attackPoint;
    private float attackRange;
    [SerializeField] LayerMask enemyLayers;
    private int damageAmount = 1;
    private bool isHit;
    private EnemyHealthController enemyHealthController;
    private Transform tempTransform;
    private PlayerMovementHandler player;

    public GameObject meleeAtackPoint;
    public GameObject rangedAttackPoint;
    void Start()
    {
        Debug.Log(weapon.name);
        player = PlayerHealthController.instance.GetComponent<PlayerMovementHandler>(); 
        if (weapon.isRanged)
        {
            attackPoint = rangedAttackPoint.transform;
            //TODO: if attack point is ranged change attack behaviour
        }  
        else
        {
            attackPoint = meleeAtackPoint.transform;
        }
        attackRange = weapon.attackRange;
        damageAmount = weapon.damageAmount;
        
    }

    void Update()
    {
        if (attackPoint == meleeAtackPoint.transform)
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
                        player.rb.velocity = new Vector2(player.rb.velocity.x, 20);
                    }
                }

            }

            if (Input.GetMouseButtonDown(0) && weapon.weaponName == "Sword")
            {  
                attackPoint.localPosition = new Vector3(0.706f, 0.073f, 0); 
                FindAndDamageEnemy();

            }
            if (Input.GetMouseButtonUp(1))
            {
                attackPoint.localPosition = new Vector3(0.706f, 0.073f, 0);
                damageAmount *= 2;
                FindAndDamageEnemy();
            }
        }
        else 
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (player.isFacingRight)
                {
                    Instantiate(weapon.projectile, attackPoint.position, Quaternion.Euler(0,0,-90));
                    weapon.moveDirection = new Vector2(transform.localScale.x, 0);
                }
                else
                {
                    Instantiate(weapon.projectile, attackPoint.position, Quaternion.Euler(0,0,90));
                    weapon.moveDirection = new Vector2(transform.localScale.x, 0);
                }
            }

        }
        
    }

    private void FindAndDamageEnemy()
    {
        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemyHit)
        {
            enemy.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
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
