using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EntityHealth : MonoBehaviour
{
    public int maxHealth, currentHealth, maxEquipmentHealth;
    int EquipmentHealth;

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
        currentHealth = maxHealth + EquipmentHealth;
    }

    public void TakeHealth(int health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth + EquipmentHealth) currentHealth = maxHealth + EquipmentHealth;
        if(currentHealth > maxHealth)
        {
            int tmp = currentHealth - maxHealth;
            if (tmp > 0) EquipmentHealth = tmp;
            else EquipmentHealth = 0;

        }
        statBar.SetHealthVisual(currentHealth, maxHealth, EquipmentHealth);
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("TakeDamage");
        transform.Bump(1.5f);
        hitParticle.Play();
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        statBar.SetHealthVisual(currentHealth, maxHealth, EquipmentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        transform.DesactiveInBump();
        isDead = true;
    }
}