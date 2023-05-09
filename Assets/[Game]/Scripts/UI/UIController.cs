using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public Slider healthSlider;
    [SerializeField] Image fadeScreen;
    [SerializeField] float fadeSpeed;
    private bool fadingToBlack;
    private bool fadingToNormal;
    public GameObject ui;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); //when we reload or load the next scene don`t destoy the player, so player don`t loose its progress 
        }
        else
        {
            Destroy(gameObject);
        }
       
    }
    void Start()
    {
        ui.gameObject.SetActive(true);
    }
    void Update()
    {
        if (fadingToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));
        
            if (fadeScreen.color.a  == 1f)
            {
                fadingToBlack = false;
            }
        }
        else if(fadingToNormal)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));
        
            if (fadeScreen.color.a  == 0f)
            {
                fadingToNormal = false;
            }
        }
    }

    public void UpdateHealth(int currentHealth, int maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    public void StartFadeToBlack()
    {
        fadingToBlack = true;
        fadingToNormal = false;
    }
    public void StartFadeToNormal()
    {
        fadingToBlack = false;
        fadingToNormal = true;
    }
}
