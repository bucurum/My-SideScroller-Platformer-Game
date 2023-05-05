using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] int damageAmount;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Damage();
        }
    }
    private void Damage()
    {
        Debug.Log("Sea");
        PlayerHealthController.instance.DamagePlayer(damageAmount);
    }
}
