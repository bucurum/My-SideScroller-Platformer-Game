using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] int damageAmount;
    EnemyMover enemyMover;

    void Start()
    {
        enemyMover = GetComponent<EnemyMover>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Damage();
            Debug.Log("general kenobi");
            enemyMover.hit = true;
        }
    }
    private void Damage()
    {
        PlayerHealthController.instance.DamagePlayer(damageAmount);
    }

}
