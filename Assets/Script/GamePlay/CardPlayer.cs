using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPlayer : MonoBehaviour
{
    public Transform attackPosRef;
    public Card chosenCard;
    [SerializeField] private TMP_Text nameText;
    public TMP_Text healthText;
    public HealthBar healthBar;

    private Tweener animationTweener;
    public TMP_Text NickName {get => nameText;}
    public bool IsReady=false;
    
    public AudioSource audioSource;
    public AudioClip damageClip;

    public float Health;
    public PlayerStats stats = new PlayerStats{
        MaxHealth=100,
        RestoreValue=5,
        DamageValue=10
    };


    private void Start()
    {
        Health = stats.MaxHealth;
    }

    public void SetStats (PlayerStats newStats, bool restoreFullHealth=false) {
        this.stats = newStats;
        if(restoreFullHealth) 
            Health = stats.MaxHealth;

        UpdateHealthBar();
    }

    public Attack? AttackValue
    {
        get => chosenCard == null ? null : chosenCard.attackValue;
    }

    public void SetChosenCard(Card newCard)
    {
        if (chosenCard != null)
        {
            chosenCard.transform.DOKill();
            chosenCard.Reset();
        }
        chosenCard = newCard;
        chosenCard.transform.DOScale(chosenCard.transform.localScale * 1.2f, 0.2f);
    }

    public void ChangeHealth(float amount)
    {
        Health += amount;
        Health = Math.Clamp(Health, 0, stats.MaxHealth);
        UpdateHealthBar();
    }

   public void UpdateHealthBar () {
        // HealthBar
        healthBar.UpdateBar(Health / stats.MaxHealth);
        // Text
        healthText.text = Health + "/" + stats.MaxHealth;
   }

    public void AnimateAttack()
    {
        animationTweener = chosenCard.transform.DOMove(attackPosRef.position, 1);
    }

    public void AnimateDamage()
    {
        audioSource.PlayOneShot(damageClip);
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
        .DOColor(Color.red, 0.1f)
        .SetLoops(3, LoopType.Yoyo)
        .SetDelay(0.2f);

    }
    internal void AnimateDraw()
    {
        var image = chosenCard.GetComponent<Image>();
        animationTweener = image
        .DOColor(Color.blue, 0.1f)
        .SetLoops(2, LoopType.Yoyo)
        .SetDelay(0.2f);
        animationTweener = chosenCard.transform
        .DOMove(chosenCard.OriginalPositition, 1)
        .SetEase(Ease.InBounce)
        .SetDelay(0.8f);
    }

    public bool IsAnimating()
    {
        return animationTweener.IsActive();
    }

    internal void Reset()
    {
        if (chosenCard != null)
        {
            chosenCard.Reset();
        }
        chosenCard = null;
    }
    public void SetClickable(bool value)
    {
        Card[] cards = GetComponentsInChildren<Card>();
        foreach (var card in cards)
        {
            card.setClickable(value);
        }
    }
}
