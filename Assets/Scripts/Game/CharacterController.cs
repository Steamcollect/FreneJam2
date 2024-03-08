using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int damage;
    public int attackPointBonnus, defensesPointBonnus, equipmentAttackPoint, equipmentDefensePoint;
    public int attackPointGiven, defensePointGiven;

    public bool canDoAction;

    public ParticleSystem healthParticle, attackParticle, defenseParticle;
    public AudioClip[] healSound, attackSound, focusSound, defenseSound;

    [HideInInspector]public EntityHealth health;
    [HideInInspector]public StatsBar statBar;
    [HideInInspector] public LoopManager loopManager;

    [HideInInspector]public CharacterController enemy;

    private void Awake()
    {
        health = GetComponent<EntityHealth>();
    }

    int CalculateAttackDamage()
    {
        int tmp = attackPointBonnus;
        attackPointBonnus = 0;

        return damage + tmp + equipmentAttackPoint;
    }
    int CalculateDamageTaken(int totalDamage)
    {
        int damage = totalDamage - defensesPointBonnus - equipmentDefensePoint;

        if(damage <= 0)
        {
            defensesPointBonnus = damage * -1;
            return 0;
        }
        else
        {
            defensesPointBonnus = 0;
            return damage;
        }
    }

    public void Attack()
    {
        if (canDoAction)
        {
            if (attackSound.Length > 0) AudioManager.instance.PlayClipAt(0, transform.position, attackSound[Random.Range(0, attackSound.Length)]);

            enemy.TakeDamage(CalculateAttackDamage());
            attackPointBonnus = 0;
            statBar.SetAttackVisual(attackPointBonnus, equipmentAttackPoint);

            StartCoroutine(loopManager.NextTurn());
        }
    }
    public void Focus()
    {
        if (canDoAction)
        {
            if (focusSound.Length > 0) AudioManager.instance.PlayClipAt(0, transform.position, focusSound[Random.Range(0, focusSound.Length)]);

            attackParticle.Play();

            attackPointBonnus += attackPointGiven;
            statBar.SetAttackVisual(attackPointBonnus, equipmentAttackPoint);

            StartCoroutine(loopManager.NextTurn());
        }        
    }
    public void Defend()
    {
        if (canDoAction)
        {
            if (defenseSound.Length > 0) AudioManager.instance.PlayClipAt(0, transform.position, defenseSound[Random.Range(0, defenseSound.Length)]);

            defenseParticle.Play();

            print(defensePointGiven);
            defensesPointBonnus += defensePointGiven;
            statBar.SetShieldVisual(defensesPointBonnus, equipmentDefensePoint);

            StartCoroutine(loopManager.NextTurn());
        }        
    }
    public void Heal()
    {
        if (canDoAction)
        {
            if (healSound.Length > 0) AudioManager.instance.PlayClipAt(0, transform.position, healSound[Random.Range(0, healSound.Length)]);

            healthParticle.Play();
            health.TakeHealth(2);
            StartCoroutine(loopManager.NextTurn());
        }
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(CalculateDamageTaken(damage));
        statBar.SetShieldVisual(defensesPointBonnus, equipmentDefensePoint);
    }
}