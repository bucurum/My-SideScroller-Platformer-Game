using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Attack")]
    public Weapon weapon;

    [HideInInspector][SerializeField] Weapon hand;
    [HideInInspector][SerializeField] Weapon sword;
    [HideInInspector][SerializeField] Weapon bow;
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
    Animator animator;

    public int combo;
    public bool attacker;

    void Start()
    {
        animator = GetComponent<Animator>();
        // currentWeapon = weapon;
        
        player = PlayerHealthController.instance.GetComponent<PlayerMovementHandler>(); 
        if (!weapon.isRanged)
        {
            attackPoint = meleeAtackPoint.transform;
        }  
        else
        {
            attackPoint = rangedAttackPoint.transform;
        }
        setWeaponValues();
    }

    void Update()
    {  
        WeaponAttack();
        ChangeWeapon();
    }

    private void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            weapon = hand;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = sword;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = bow;
        }
    }

    private void WeaponAttack()
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
        else if(weapon.name == "Bow")
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
        else if (weapon.name == "Hand")
        {
            if (Input.GetMouseButtonDown(0) && !attacker)
            {
                attacker = true;
                animator.SetTrigger(""+combo);
            }
        }
    }

    public void StartCombo()
    {
        attacker = false;
        if (combo < 3)
        {
            combo++;
        }
    }
    public void FinisAnimation()
    {
        attacker = false;
        combo = 0;
    }

    private void setWeaponValues()
    {
        attackRange = weapon.attackRange;
        damageAmount = weapon.damageAmount;
        animator.runtimeAnimatorController = weapon.animatorOverride;
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
