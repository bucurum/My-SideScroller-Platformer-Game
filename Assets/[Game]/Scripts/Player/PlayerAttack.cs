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
    private EnemyHealthController enemyHealthController;
    private Transform tempTransform;
    private PlayerMovementHandler player;

    [HideInInspector] public GameObject meleeAtackPoint;
    [HideInInspector] public GameObject rangedAttackPoint;

    private float holdDownStartTime;
    private float holdDownTime;

    void Start()
    {
        
        Debug.Log(weapon.name);
        player = PlayerHealthController.instance.GetComponent<PlayerMovementHandler>(); 
        if (weapon.isRanged)
        {
            attackPoint = rangedAttackPoint.transform;
        }  
        else
        {
            attackPoint = meleeAtackPoint.transform;
        }
        setWeaponValues();
    }

    void Update()
    {
        ChangeWeapon();
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     weapon = Resources.Load<Weapon>("Assets/[Game]/Scripts/Weapons/Sword.asset");
        // }
        // if (Input.GetKeyDown(KeyCode.Alpha2))
        // {
        //     weapon = Resources.Load<Weapon>("Assets/[Game]/Scripts/Weapons/Bow.asset");
        // }
        
    }

    private void ChangeWeapon()
    {
        if (weapon.name == "Sword")
        {
            
            attackPoint = meleeAtackPoint.transform;
            setWeaponValues();
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
            if (Input.GetMouseButtonDown(1))
            {
                holdDownStartTime = Time.time;
                player.heavyAttackHolded = false;
            }
            if (Input.GetMouseButtonUp(1))
            {

                holdDownTime = Time.time - holdDownStartTime;
                if (holdDownTime > .5)
                {
                    player.heavyAttackHolded = true;
                    attackPoint.localPosition = new Vector3(0.706f, 0.073f, 0);
                    damageAmount *= 2;
                    FindAndDamageEnemy();
                    damageAmount /= 2;

                }

            }
        }
        else
        {
            attackPoint = rangedAttackPoint.transform;
            setWeaponValues();
            if (Input.GetMouseButtonUp(0))
            {
                if (!player.isFacingRight)
                {
                    Instantiate(weapon.projectile, attackPoint.position, Quaternion.Euler(0, 0, 90));
                    weapon.moveDirection = new Vector2(transform.localScale.x, 0);
                }
                else
                {
                    Instantiate(weapon.projectile, attackPoint.position, Quaternion.Euler(0, 0, -90));
                    weapon.moveDirection = new Vector2(transform.localScale.x, 0);
                }
            }

        }
    }

    private void setWeaponValues()
    {
        attackRange = weapon.attackRange;
        damageAmount = weapon.damageAmount;
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
