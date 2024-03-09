using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    public int damage;
    public int equipmentHealthPointGiven, attackPointBonnus, defensesPointBonnus, equipmentAttackPoint, equipmentAttackPointGiven, equipmentDefensePoint, equipmentDefensePointGiven;
    public int attackPointGiven, defensePointGiven, healPointGiven;

    public bool canDoAction;

    public Sprite idleSprite, hitSprite, attackSprite;
    public ParticleSystem healthParticle, attackParticle, defenseParticle;
    public AudioClip[] healSound, attackSound, focusSound, defenseSound;

    [HideInInspector]public EntityHealth health;
    [HideInInspector]public StatsBar statBar;
    [HideInInspector] public LoopManager loopManager;

    [HideInInspector]public CharacterController enemy;
    SpriteRenderer graphics;

    private void Awake()
    {
        graphics = GetComponent<SpriteRenderer>();
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

            StartCoroutine(ChangeSprite(attackSprite));
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

            attackPointBonnus += attackPointGiven + equipmentAttackPointGiven;
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
            defensesPointBonnus += defensePointGiven + equipmentDefensePointGiven;
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
            health.TakeHealth(healPointGiven + equipmentHealthPointGiven);
            StartCoroutine(loopManager.NextTurn());
        }
    }

    public void TakeDamage(int damage)
    {
        StartCoroutine(ChangeSprite(hitSprite));
        health.TakeDamage(CalculateDamageTaken(damage));
        statBar.SetShieldVisual(defensesPointBonnus, equipmentDefensePoint);
    }

    public void SetStatBar()
    {
        statBar.SetHealthVisual(health.maxHealth, health.maxHealth);
        statBar.SetShieldVisual(defensesPointBonnus, equipmentDefensePoint);
        statBar.SetAttackVisual(attackPointBonnus, equipmentAttackPoint);
    }

    IEnumerator ChangeSprite(Sprite newSprite)
    {
        graphics.sprite = newSprite;

        yield return new WaitForSeconds(.3f);
        graphics.sprite = idleSprite;
    }
}