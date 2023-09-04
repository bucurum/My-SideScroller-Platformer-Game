using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public GameObject projectile;
    public string weaponName;
    public int damageAmount;
    public Transform attackPoint;
    public float attackRange;
    public bool isRanged;
    public WeaponType weaponType; // TODO Add weaponType to declarations
    public Vector2 moveDirection;

    public AnimatorOverrideController animatorOverride = null;

/*
    public virtual void Attack(PlayerAttack player){
        // 1 Get player stats
        // 2 Do physics collider calculations
        // TODO Add attack calculations, and deal damage
    }
*/
    //TODO: if attackPoint is null use the player`s attack point
}

public enum WeaponType 
{
    Bow,
    Hand,
    Sword
}