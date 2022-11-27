using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public enum ObjectType
{
    PLAYER = 0,
    ENEMY = 1,
    RESOURCE = 2
}

public class Health : MonoBehaviour
{
    [SerializeField] private ObjectType type;
    private int health;

    [SerializeField] private int MAX_HEALTH = 100;
    
    public MMFeedbacks feelFeedback;
    public ObjectType GetObjectType => type;
    public int GetHealthValue => health;
    private bool isChangedFeel;
    
    private void Start()
    {
        health = MAX_HEALTH;
        isChangedFeel = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Damage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            Heal(10);
        }
    }

    public void SetHealth(int maxHealth, int health)
    {
        this.MAX_HEALTH = maxHealth;
        this.health = health;
    }

    // Added for Visual Indicators
    private IEnumerator VisualIndicator(Color color)
    {
        this.gameObject.GetComponent<SpriteRenderer>().color = color;
        yield return new WaitForSeconds(0.15f);
        this.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
    }

    
    public void Damage(int amount)
    {
        if(amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative Damage");
        }
        
        this.health -= amount;
        StartCoroutine(VisualIndicator(Color.red)); // Added for Visual Indicators

        if (health <= 0)
        {
            Die();
        }
        else
        {
            if (type == ObjectType.PLAYER)
            {
                // HP.Instance.Heal();
                if (health < MAX_HEALTH / 3  && !isChangedFeel)
                {
                    TreeHandler.Instance.ChangeFeel(isChangedFeel);
                    isChangedFeel = true;
                    Debug.Log(isChangedFeel);
                }
                
                if (health < MAX_HEALTH / 2)
                {
                    TreeHandler.Instance.IsLow();
                }
            }
        }
    }

    public void Heal(int amount)
    {
        if (type != ObjectType.PLAYER) return;
        if (amount < 0)
        {
            throw new System.ArgumentOutOfRangeException("Cannot have negative healing");
        }

        bool wouldBeOverMaxHealth = health + amount > MAX_HEALTH;
        StartCoroutine(VisualIndicator(Color.green)); // Added for Visual Indicators
        TreeHandler.Instance.IsHealth();
        // HP.Instance.Heal();
        if (wouldBeOverMaxHealth)
        {
            this.health = MAX_HEALTH;
        }
        else
        {
            this.health += amount;
        }

        if (this.health >= MAX_HEALTH / 2)
        {
            TreeHandler.Instance.IsNormal();
        }

        if (this.health >= MAX_HEALTH / 3 && isChangedFeel)
        {
            TreeHandler.Instance.ChangeFeel(isChangedFeel);
            isChangedFeel = false;
        }
    }

    private void Die()
    {
        // Debug.Log("I am Dead!");
        if (type == ObjectType.ENEMY)
        {
            if (EnemySpawner.Instance.spawnedEnemy.Contains(this.GameObject()))
            {
                EnemySpawner.Instance.spawnedEnemy.Remove(this.GameObject());
            }
        }
        else if (type == ObjectType.RESOURCE)
        {
            if (EnemySpawner.Instance.spawnedResource.Contains(this.GameObject()))
            {
                EnemySpawner.Instance.spawnedResource.Remove(this.GameObject());
            }

            feelFeedback?.PlayFeedbacks();
        }
        else if (type == ObjectType.PLAYER)
        {
            Debug.LogError("IsDead");
            TreeHandler.Instance.IsDead();
            StartCoroutine(DeadLoading());
            return;
        }
        Destroy(gameObject);
    }

    IEnumerator DeadLoading()
    {
        EnemySpawner.Instance.ShowScore();
        EnemySpawner.Instance.Restart();
        yield return new WaitForSeconds(3);
        TreeHandler.Instance.Reset();
        EnemySpawner.Instance.ResetUI();
        if (isChangedFeel)
        {
            TreeHandler.Instance.ChangeFeel(isChangedFeel);
            isChangedFeel = false;
        }
        health = MAX_HEALTH;
        isChangedFeel = false;
    }
}
