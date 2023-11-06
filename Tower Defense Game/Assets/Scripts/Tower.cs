using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public static event Action<Tower> TowerDestroyed;
    public static event Action<int> HealthChanged; 

    [SerializeField] internal float maxHealth;

    private float health;

    private void Start()
    {
        health = maxHealth;

        Enemy.ReachedTower += TowerReached;
        HealthChanged?.Invoke((int)health);
    }
    private void TakeDamage(float damage)
    {
        health -= damage;

        HealthChanged?.Invoke((int)health);

        float percentage = health / maxHealth * 100;

        if (percentage >= 80)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animations/Boombox/Normal");
        }
        else if (percentage >= 40)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animations/Boombox/Damaged");
        }
        else if (percentage >= 25)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animations/Boombox/Breaking");
        }
        else if (percentage > 0)
        {
            GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Animations/Boombox/Broken");     
        }
        else
        {
            TowerDestroyed?.Invoke(this);
        }
    }

    private void TowerReached(Enemy enemy)
    {
        enemy.TakeDamage(enemy.maxHealth);
        TakeDamage(enemy.towerDamage);
    }
}
