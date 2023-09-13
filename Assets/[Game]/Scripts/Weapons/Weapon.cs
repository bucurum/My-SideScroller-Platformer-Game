using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject projectile;
    public string weaponName;
    public int damageAmount;
    public int heavyDamageAmount;
    public float attackRange;
    public bool isRanged = false;
    public WeaponType weaponType; // TODO Add weaponType to declarations
    public Vector2 moveDirection;
    
    [SerializeField] LayerMask enemyLayers;

    public AnimatorOverrideController animatorOverride = null;
    
    public virtual void Attack(PlayerAttack player){ //
        // 1 Get player stats
        // 2 Do physics collider calculations
        // TODO Add attack calculations, and deal damage

        
        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(player.GetAttackPoint().position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemyHit)
        {
            enemy.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }
    }

    public virtual void HeavyAttack(PlayerAttack player)
    {

        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(player.GetAttackPoint().position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemyHit)
        {
            enemy.GetComponent<EnemyHealthController>().DamageEnemy(heavyDamageAmount);
        }
    }

    //TODO: if attackPoint is null use the player`s attack point
}

public enum WeaponType 
{
    Bow,
    Hand,
    Sword,
    Sword2
}