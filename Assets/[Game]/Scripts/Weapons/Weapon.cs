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
    public Vector2 moveDirection;

    public AnimatorOverrideController animatorOverride = null;


    //TODO: if attackPoint is null use the player`s attack point
}
