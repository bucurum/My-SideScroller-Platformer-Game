using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterimage : MonoBehaviour
{
    
    [SerializeField] private float activeTime = .1f;
    private float timeActivated;
    private float alpha;
    [SerializeField] private float alphaSet = .8f;
    private float alphaMultipler = .85f;

    private Transform player;

    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerSpriteRender;

    private Color color;

    void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSpriteRender = player.GetComponentInChildren<SpriteRenderer>();

        alpha = alphaSet;
        spriteRenderer.sprite = playerSpriteRender.sprite;
        
        transform.position = player.position;
        transform.rotation = player.rotation;
        
        timeActivated = Time.time;
    }

    void Update()
    {
        alpha *= alphaMultipler;
        color = new Color(1f,1f,1f,alpha);   
        spriteRenderer.color = color;

        if (Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }

    
}
