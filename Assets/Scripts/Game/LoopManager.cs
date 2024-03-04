using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopManager : MonoBehaviour
{
    public GameObject playerPrefabs;
    public GameObject[] enemysPrefabs;

    EnemyAI ai;

    public Vector2 playerPos, enemyPos;

    bool isPlayerTurn = false;
    CharacterController player, enemy;

    ScoreManager scoreManager;

    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
    }

    private void Start()
    {
        ai = new EnemyAI();

        player = Instantiate(playerPrefabs, playerPos, Quaternion.identity).GetComponent<CharacterController>();
        enemy = Instantiate(enemysPrefabs[Random.Range(0, enemysPrefabs.Length)], enemyPos, Quaternion.identity).GetComponent<CharacterController>();

        StartCoroutine(NextTurn());
    }

    IEnumerator NextTurn()
    {
        isPlayerTurn = !isPlayerTurn;

        if (!isPlayerTurn)
        {
            yield return new WaitForSeconds(.8f);

            switch (ai.SelectAction())
            {
                case 0:
                    Attack();
                    break;
                case 1:
                    Defend();
                    break;
                case 2:
                    Focus();
                    break;
                case 3:
                    Heal();
                    break;
            }
        }
    }

    public void Attack()
    {
        if (isPlayerTurn)
        {
            enemy.health.takeDamage((int)player.CalculateAttackDamage());
            enemy.hitParticle.Play();
            if (enemy.health.isDead)
            {
                scoreManager.AddScore(5);
                enemy.GetComponent<Animator>().SetTrigger("Die");
                print("Enemy Dead");
                return;
            }
        }
        else
        {
            player.health.takeDamage((int)enemy.CalculateAttackDamage());
            player.hitParticle.Play();
            if (player.health.isDead)
            {
                player.GetComponent<Animator>().SetTrigger("Die");
                print("Player Dead");
                return;
            }
        }

        StartCoroutine(NextTurn());
    }
    public void Defend()
    {
        if (isPlayerTurn)
        {
            player.AddDefensePoint(30);
            player.defenseParticle.Play();
        }
        else
        {
            enemy.AddDefensePoint(30);
            enemy.defenseParticle.Play();
        }

        StartCoroutine(NextTurn());
    }
    public void Focus()
    {
        if (isPlayerTurn)
        {
            player.AddAttackPoint(20);
            player.attackParticle.Play();
        }
        else
        {
            enemy.AddAttackPoint(20);
            enemy.attackParticle.Play();
        }

        StartCoroutine(NextTurn());
    }
    public void Heal()
    {
        if (isPlayerTurn)
        {
            player.health.TakeHealth(10);
            player.healthParticle.Play();
        }
        else
        {
            enemy.health.TakeHealth(10);
            enemy.healthParticle.Play();
        }

        StartCoroutine(NextTurn());
    }
}