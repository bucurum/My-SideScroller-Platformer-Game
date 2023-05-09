using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    int currentHealth;
    int maxHealth;
    [SerializeField] RectTransform topBar;
    [SerializeField] RectTransform bottomBar;

    private float fullWidth;
    private float TargetWidth => currentHealth * fullWidth / maxHealth;
    [SerializeField] private float animationSpeed = 10f;

    private Coroutine adjustBarWidthCoroutine;

    void Start()
    {
        fullWidth = topBar.rect.width;
        maxHealth = GetComponentInParent<EnemyHealthController>().totalHealth;
        currentHealth = maxHealth;
    }
    void Update()
    {
        if (transform.parent.gameObject.transform.localScale != Vector3.one)
        {
            this.gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            this.gameObject.transform.localScale = Vector3.one;
        }
    }
    private IEnumerator AdjustBarWidth(int amount)
    {
        var suddenChangeBar = amount >= 0 ? bottomBar : topBar;
        var slowChangeBar = amount >= 0 ? topBar : bottomBar;
        suddenChangeBar.SetWidth(TargetWidth);
        while(Mathf.Abs(suddenChangeBar.rect.width - slowChangeBar.rect.width) > 1f)
        {
            slowChangeBar.SetWidth(Mathf.Lerp(slowChangeBar.rect.width, TargetWidth, Time.deltaTime * animationSpeed));
            yield return null;
        }
        slowChangeBar.SetWidth(TargetWidth);
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        if (adjustBarWidthCoroutine != null)
        {
            StopCoroutine(adjustBarWidthCoroutine);
        }
        adjustBarWidthCoroutine = StartCoroutine(AdjustBarWidth(amount));
    }
 }
