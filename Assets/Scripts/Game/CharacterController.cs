using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public float damage;
    public float attackPointBonnus, defensesPointBonnus;

    public ParticleSystem healthParticle, attackParticle, defenseParticle, hitParticle;

    [HideInInspector]public EntityHealth health;

    private void Awake()
    {
        health = GetComponent<EntityHealth>();
    }

    public float CalculateAttackDamage()
    {
        float tmp = attackPointBonnus;
        attackPointBonnus = 0;

        return damage + tmp;
    }
    public float calculateDamageTaken(float totalDamage)
    {
        return totalDamage - (totalDamage * (float)(defensesPointBonnus / 100f));
    }

    public void AddAttackPoint(int points)
    {
        attackPointBonnus += points;
    }
    public void AddDefensePoint(int points)
    {
        defensesPointBonnus += points;
    }
}