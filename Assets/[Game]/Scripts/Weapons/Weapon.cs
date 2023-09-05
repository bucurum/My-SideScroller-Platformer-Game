using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject projectile;
    public string weaponName;
    public int damageAmount;
    public float attackRange;
    public bool isRanged = false;
    public WeaponType weaponType; // TODO Add weaponType to declarations
    public Vector2 moveDirection;
     public Transform attackPoint;
    
    private PlayerAttack playerAttack;
    [SerializeField] LayerMask enemyLayers;

    public AnimatorOverrideController animatorOverride = null;

    private void Awake() {
        playerAttack = PlayerHealthController.instance.GetComponent<PlayerAttack>();

    }
    
    public virtual void Attack(){ //PlayerAttack player
        // 1 Get player stats
        // 2 Do physics collider calculations
        // TODO Add attack calculations, and deal damage

        
        
        Collider2D[] enemyHit = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in enemyHit)
        {
            enemy.GetComponent<EnemyHealthController>().DamageEnemy(damageAmount);
        }
    }

    //TODO: if attackPoint is null use the player`s attack point
}

public enum WeaponType 
{
    Bow,
    Hand,
    Sword
}