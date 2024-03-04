using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public int maxHealth, currentHealth;

    public bool isDead = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeHealth(int health)
    {
        currentHealth += health;
        if(currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void takeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
    }
}