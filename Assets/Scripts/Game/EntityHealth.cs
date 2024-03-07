using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public int maxHealth, currentHealth;

    public StatsBar statBar;
    public ParticleSystem hitParticle;
    Animator anim;

    public bool isDead = false;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        statBar.SetHealthVisual(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("TakeDamage");
        transform.Bump(1.5f);
        hitParticle.Play();
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        statBar.SetHealthVisual(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
        isDead = true;
    }
}