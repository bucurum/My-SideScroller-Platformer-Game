using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Attack")]
    public Weapon weapon;
    Animator animator;

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
    public float attackDelayTime;

    [Header("Combo")]
    

    // Insert your character's animator reference here
    private float lastAttackTime; // Last time an attack occurred
    private int comboCount; // Number of consecutive attacks in the combo

    // Three different attack animation names
    public string attackAnimation1 = "HandCombat1";
    public string attackAnimation2 = "HandCombat2";
    public string attackAnimation3 = "HandCombat3";

    internal bool canAttack = true;

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
        SetWeaponValues();
    }

    void Update()
    {  
        WeaponAttack();
        ChangeWeapon();
        Debug.Log(canAttack);
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
        if (canAttack)
        {
            
            if (weapon.name == "Sword")
            {
                attackPoint = meleeAtackPoint.transform;
                SetWeaponValues();
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
                SetWeaponValues();
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
                attackPoint.localPosition = new Vector3(0.706f, 0.073f, 0);
                SetWeaponValues();

                if (Input.GetMouseButtonDown(0))
                {

                    HandleMeleeAttack();
                }
            }
        }
    }

    private void SetWeaponValues()
    {
        attackRange = weapon.attackRange;
        damageAmount = weapon.damageAmount;
        if (weapon.animatorOverride != null)
        {
            animator.runtimeAnimatorController = weapon.animatorOverride;
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

    private void HandleMeleeAttack()
    {
        // Check if enough time has passed to reset the combo chain
        if (Time.time - lastAttackTime > .5f)
        {
            comboCount = 0;
        }

        // Increment the combo count (limited by maxComboCount)
        if (comboCount < 3)
        {
            comboCount++;
        }
        else
        {
            comboCount = 0;
        }
        

        // Play the corresponding attack animation based on the combo count
        switch (comboCount)
        {
            case 1:
                PlayAttackAnimation(attackAnimation1);
                FindAndDamageEnemy();
                break;
            case 2:
                PlayAttackAnimation(attackAnimation2);
                FindAndDamageEnemy();
                break;
            case 3:
                PlayAttackAnimation(attackAnimation3);
                FindAndDamageEnemy();
                break;
        }

        lastAttackTime = Time.time;
    }

    private void PlayAttackAnimation(string animName)
    {
        // Play the specified attack animation in the animator
        if (animator != null)
        {
            animator.SetTrigger(animName);
        }
    }

}
