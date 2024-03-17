using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterController : MonoBehaviour
{
    public int damage;
    [HideInInspector] public int equipmentHealthPoint, attackPointBonnus, defensesPointBonnus, equipmentAttackPoint, equipmentDefensePoint;
    [HideInInspector] public int attackPointGiven, defensePointGiven, healPointGiven;

    public int healPotionCount, defensePotionCount, attackPotionCount;

    [HideInInspector] public bool canDoAction;

    [Header("Sprite references")]
    public Sprite idleSprite;
    public Sprite hitSprite;
    public Sprite attackSprite;
    [Header("Particle references")]
    public ParticleSystem healthParticle;
    public ParticleSystem attackParticle;
    public ParticleSystem defenseParticle;
    [Header("Sound references")]
    public AudioClip[] healSound;
    public AudioClip[] attackSound;
    public AudioClip[] focusSound;
    public AudioClip[] defenseSound;

    [Header("Potion panel references")]
    public GameObject potionPanel;
    public TMP_Text healthPotionCountTxt, defensePotionCountTxt, attackPotionCountTxt;

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
        int damageBlocked = defensesPointBonnus + equipmentDefensePoint;
        if(this == loopManager.player)
        {
            if (damageBlocked > totalDamage) damageBlocked = totalDamage;
            loopManager.damagedBlocked = damageBlocked;
        }

        int damage = totalDamage - damageBlocked;

        if(damage <= 0)
        {
            defensesPointBonnus = damage * -1;
            return 0;
        }
        else
        {
            if (this == loopManager.player) loopManager.damagedReceived = damage;
            defensesPointBonnus = 0;

            //print(totalDamage + ", "+ damage);

            return damage;
        }
    }

    public void Attack()
    {
        if (canDoAction)
        {
            if (attackSound.Length > 0) AudioManager.instance.PlayClipAt(0, transform.position, attackSound[Random.Range(0, attackSound.Length)]);

            StartCoroutine(ChangeSprite(attackSprite));
            int damage = CalculateAttackDamage();
            if (this == loopManager.player) loopManager.damagedInflicted += damage;
            enemy.TakeDamage(damage);
            attackPointBonnus = 0;
            statBar.SetAttackVisual(this.damage, equipmentAttackPoint);

            StartCoroutine(loopManager.NextTurn());
        }
    }
    public void Focus(int attackGiven)
    {
        if (canDoAction)
        {
            if (focusSound.Length > 0) AudioManager.instance.PlayClipAt(0, transform.position, focusSound[Random.Range(0, focusSound.Length)]);

            attackParticle.Play();

            attackPointBonnus += attackGiven == 0 ? attackPointGiven : attackGiven;
            statBar.SetAttackVisual(attackPointBonnus + damage, equipmentAttackPoint);

            StartCoroutine(loopManager.NextTurn());
        }        
    }
    public void Defend(int defenseGiven)
    {
        if (canDoAction)
        {
            if (defenseSound.Length > 0) AudioManager.instance.PlayClipAt(0, transform.position, defenseSound[Random.Range(0, defenseSound.Length)]);

            defenseParticle.Play();

            defensesPointBonnus += defenseGiven == 0 ? defensePointGiven : defenseGiven;
            statBar.SetShieldVisual(defensesPointBonnus, equipmentDefensePoint);

            StartCoroutine(loopManager.NextTurn());
        }        
    }
    public void Heal(int healthGiven)
    {
        if (healSound.Length > 0) AudioManager.instance.PlayClipAt(0, transform.position, healSound[Random.Range(0, healSound.Length)]);
        healthParticle.Play();

        int healthRecorvery = healthGiven == 0 ? healPointGiven + equipmentHealthPoint : healthGiven;
        if (health.currentHealth + healthRecorvery > health.maxHealth) healthRecorvery = health.maxHealth - health.currentHealth;

        if (this == loopManager.player) loopManager.lifeRecorvery = healthRecorvery;

        health.TakeHealth(healthRecorvery);
        StartCoroutine(loopManager.NextTurn());
    }
    public void UsePotionButton()
    {
        if(this == loopManager.player)
        {
            // Set potion panel
            potionPanel.SetActive(!potionPanel.activeSelf);
            potionPanel.transform.Bump(1.1f);

            healthPotionCountTxt.text = "x" + healPotionCount.ToString();
            defensePotionCountTxt.text = "x" + defensePotionCount.ToString();
            attackPotionCountTxt.text = "x" + attackPotionCount.ToString();
        }        
    }

    public void HealPotion()
    {
        if (canDoAction && healPotionCount > 0)
        {
            healPotionCount--;
            Heal(5);
            UsePotionButton();
        }
    }
    public void DefensePotion()
    {
        if(defensePotionCount > 0)
        {
            defensePotionCount--;
            Defend(5);
            UsePotionButton();
        }
    }
    public void AttackPotion()
    {
        if(attackPotionCount > 0)
        {
            attackPotionCount--;
            Focus(5);
            UsePotionButton();
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
        statBar.SetAttackVisual(attackPointBonnus + damage, equipmentAttackPoint);
    }

    IEnumerator ChangeSprite(Sprite newSprite)
    {
        graphics.sprite = newSprite;

        yield return new WaitForSeconds(.3f);
        graphics.sprite = idleSprite;
    }
}