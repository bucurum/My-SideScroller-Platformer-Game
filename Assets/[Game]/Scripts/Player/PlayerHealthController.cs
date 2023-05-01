using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [Header("Health")]
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;

    [Header("Invincibility")]
    [SerializeField] float invincibilityLength;
    private float invincibilityCounter;
    [SerializeField] float flashLength;
    private float flashCounter;
    [SerializeField] SpriteRenderer[] playerSprites;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // if (invincibilityCounter > 0)
        // {
        //     invincibilityCounter -= Time.deltaTime;

        //     flashCounter -= Time.deltaTime;
        //     if (flashCounter <= 0)
        //     {
        //         foreach (SpriteRenderer sprites in playerSprites)
        //         {
        //             sprites.enabled = true;
        //         }
        //         flashCounter = 0;
        //     }
        // }    
    }

    public void DamagePlayer(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

}
