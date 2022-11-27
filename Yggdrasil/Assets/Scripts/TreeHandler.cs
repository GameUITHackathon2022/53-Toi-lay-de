using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class TreeHandler : Singleton<TreeHandler>
{
    [SerializeField] private int treeSize;
    [SerializeField] private float scaleValue;
    [SerializeField] private float scaleScreenValue;
    [SerializeField] private ParticleSystem healParticals;
    [SerializeField] private AudioSource backgroundMusic;
    public int GetTreeSize => treeSize;
    public Health treeHealth;
    public GameObject shieldControler;
    public Animator animator;
    public TMPro.TextMeshProUGUI healUI;
    public MMFeedbacks LandingFeedback;
    public MMFeedbacks RemoveFeel;
    private int preSize;
    private void Start()
    {
        treeSize = 1;
        preSize = 1;
        IsNormal();
    }

    private void Update()
    {
        if (treeSize % 5 == 0 && treeSize != preSize)
        {
            preSize = treeSize;
            var scale = this.gameObject.transform.localScale;
            this.gameObject.transform.localScale = new Vector3(scale.x + scaleValue, scale.y + scaleValue, scale.z + scaleValue);
            EnemySpawner.Instance.UpdateScreenSize(scaleScreenValue);
        }
        
        healUI.text = treeHealth.GetHealthValue.ToString();
    }

    public void Reset()
    {
        treeSize = 1;
        preSize = 1;
        IsReset();
    }
    
    public void IsNormal()
    {
        animator.SetBool("IsLowHP", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsReset", false);
    }

    public void IsReset()
    {
        animator.SetBool("IsLowHP", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsReset", true);
    }
    
    public void IsLow()
    {
        animator.SetBool("IsLowHP", true);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsReset", false);
    }
    
    public void IsDead()
    {
        animator.SetBool("IsLowHP", true);
        animator.SetBool("IsDead", true);
        animator.SetBool("IsReset", false);
    }
    
    public void AddCoin()
    {
        treeSize++;
    }

    public void IsLowMode(bool value)
    {
        if (value)
        {
            backgroundMusic.DOPitch(0.8f, 1);
        }
        else
        {
            backgroundMusic.DOPitch(1f, 1);
        }
    }
    
    
    public void HitAxe()
    {
        LandingFeedback?.PlayFeedbacks();
    }

    public void ChangeFeel(bool whatfeel)
    {
        IsLowMode(!whatfeel);
        RemoveFeel?.PlayFeedbacks();
    }

    public void IsHealth()
    {
        healParticals.Play();
    }
    
}
