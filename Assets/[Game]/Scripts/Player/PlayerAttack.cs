using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Player Attack")]
    public Weapon weapon;
    Animator animator;


    public Transform attackPoint;
    private float attackRange;
    
    private int damageAmount = 1;
    private PlayerMovementHandler player;

    [HideInInspector] public GameObject meleeAtackPoint;
    [HideInInspector] public GameObject rangedAttackPoint;

    [HideInInspector] public float holdDownStartTime;
    [HideInInspector] public float holdDownTime;
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
        player = GetComponent<PlayerMovementHandler>();
        // currentWeapon = weapon;
        
        // player = PlayerHealthController.instance.GetComponent<PlayerMovementHandler>(); 
        // if (!weapon.isRanged)
        // {
        //     attackPoint = meleeAtackPoint.transform;
        // }  
        // else
        // {
        //     attackPoint = rangedAttackPoint.transform;
        // }
        SetWeaponValues();
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
            weapon = ItemDatabase.Instance.GetWeaponOfType(WeaponType.Hand);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            weapon = ItemDatabase.Instance.GetWeaponOfType(WeaponType.Sword);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            weapon = ItemDatabase.Instance.GetWeaponOfType(WeaponType.Bow);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            weapon = ItemDatabase.Instance.GetWeaponOfType(WeaponType.Sword2);
        }
    }

    private void WeaponAttack()
    {
        if (canAttack)
        { 
            if (weapon.weaponType == WeaponType.Sword)
            {
                SetWeaponValues();
                if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.S) && player.isfallAttacking)
                {
                    attackPoint.localPosition = new Vector3(0, -1, 0);
                    weapon.Attack(this);
                }

                if (Input.GetMouseButtonDown(0))
                {
                    attackPoint.localPosition = new Vector3(0.706f, 0.073f, 0);
                    weapon.Attack(this);
                    
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
                        // attackPoint.localPosition = new Vector3(0.706f, 0.073f, 0);
                        weapon.HeavyAttack(this);
                    }
                }
            }
            else if(weapon.weaponType == WeaponType.Bow)
            {
                SetWeaponValues();
                if (Input.GetMouseButtonDown(0))
                {
                    holdDownStartTime = Time.time;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    holdDownTime = Time.time - holdDownStartTime;
                    Instantiate(weapon.projectile, attackPoint.position, Quaternion.identity);
                    weapon.moveDirection = new Vector2(transform.localScale.x, 0);   
                }

            }
            else if (weapon.weaponType == WeaponType.Hand)
            {
                attackPoint.localPosition = new Vector3(0.706f, 0.073f, 0);
                SetWeaponValues();

                if (Input.GetMouseButtonDown(0))
                {

                    HandleMeleeAttack();
                }
            }
            else if(weapon.weaponType == WeaponType.Sword2)
            {
                
                SetWeaponValues();
                if (Input.GetMouseButtonDown(0))
                {

                    HandleMeleeAttack();
                }
            }

        }
    }

    public Transform GetAttackPoint()
    {
        if (attackPoint == null)
        {
            Debug.Log("Attack Point is null!!!");
            return transform;
        }
        return attackPoint;
    }

    private void SetWeaponValues()
    {
        if (weapon.isRanged)
        {
            attackPoint = rangedAttackPoint.transform;
        }
        else
        {
            attackPoint = meleeAtackPoint.transform;
        }
        attackRange = weapon.attackRange;
        damageAmount = weapon.damageAmount;
        if (weapon.animatorOverride != null)
        {
            animator.runtimeAnimatorController = weapon.animatorOverride;
        }
        
    }

    // private void FindAndDamageEnemy()
    // {
    //     Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

    //     foreach (Collider2D enemy in enemyHit)
    //     {
    //         enemy.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
    //     }
    // }

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
                weapon.Attack(this);
                break;
            case 2:
                PlayAttackAnimation(attackAnimation2);
                weapon.Attack(this);
                break;
            case 3:
                PlayAttackAnimation(attackAnimation3);
                weapon.Attack(this);
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
