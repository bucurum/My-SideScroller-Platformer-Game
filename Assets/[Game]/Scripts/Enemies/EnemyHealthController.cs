using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public static EnemyHealthController instance;
    [SerializeField] int totalHealth;
    private int currrentHealth;
    public GameObject deathEffect;
    public Animator animator;
    void Start()
    {
        currrentHealth = totalHealth;
    }
    public void DamageEnemy(int damageAmount)
    {
        currrentHealth -= damageAmount;
        if (animator != null)
        {
            animator.SetTrigger("takeDamage");
        }
        

        if (currrentHealth <= 0)
        {
            if (deathEffect != null)
            {
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            }
            if (gameObject.CompareTag("Spawner"))
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
        }
    }
}
