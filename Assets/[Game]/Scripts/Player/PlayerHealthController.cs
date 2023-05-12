using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;

    [Header("Health")]
    public int maxHealth;
    [SerializeField]
    public int currentHealth;

    [Header("Invincibility")]
    [SerializeField] float invincibilityLenght;
    private float invincibilityCounter;
    [SerializeField] float flashLenght;
    private float flashCounter;

    public SpriteRenderer spriteRenderer;
    

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
        UIController.instance.UpdateHealth(currentHealth, maxHealth);
    }

    void Update()
    {
        if (invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
        
            flashCounter -=Time.deltaTime;

            if (flashCounter <0)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                flashCounter = flashLenght;
            }

            if (invincibilityCounter <= 0)
            { 
                spriteRenderer.enabled = true;
                flashCounter = 0;  
            }
        }
    }

    public void DamagePlayer(int damageAmout)
    {
        if (invincibilityCounter <= 0)
        {
            currentHealth -= damageAmout;

            if (currentHealth <= 0)
            {
                currentHealth = 0;

                gameObject.SetActive(false);
            }
            else
            {
                invincibilityCounter = invincibilityLenght;
            }
            UIController.instance.UpdateHealth(currentHealth, maxHealth);

        }
        
    }

}
