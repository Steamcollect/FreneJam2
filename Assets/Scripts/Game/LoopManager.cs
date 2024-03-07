using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoopManager : MonoBehaviour
{
    public GameObject playerGO;

    // Enemy stats
    public List<GameObject> enemysPrefabs = new List<GameObject>();
    EnemyAI enemyAI;
    public Vector2 enemyPos;

    CharacterController player, enemy;
    public StatsBar playerStatBar, enemyStatBar;

    public Button[] playerButtons;
    List<InteractiveButton> playerInteractiveButtons = new List<InteractiveButton>();

    public bool isPlayerTurn = false;

    public List<ItemData> items = new List<ItemData>();

    public int itemDropPercentage;
    ScoreManager scoreManager;
    Inventory inventory;

    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
        inventory = GetComponent<Inventory>();

        for (int i = 0; i < playerButtons.Length; i++)
        {
            playerInteractiveButtons.Add(playerButtons[i].GetComponent<InteractiveButton>());
        }
    }

    private void Start()
    {
        // Set player stats
        player = playerGO.GetComponent<CharacterController>();
        player.loopManager = this;
        player.statBar = playerStatBar;
        player.health.statBar = playerStatBar;
        playerStatBar.SetHealthVisual(player.health.maxHealth, player.health.maxHealth);
        playerStatBar.SetShieldVisual(player.defensesPointBonnus);
        playerStatBar.SetAttackVisual(player.attackPointBonnus);

        enemyAI = new EnemyAI();

        CreateNewEnemy();

        player.enemy = enemy;
        inventory.playerController = player;

        StartCoroutine(NextTurn());
    }

    public IEnumerator NextTurn()
    {
        if (enemy.health.isDead)
        {
            scoreManager.AddScore(5);
            if(Random.Range(0,100) <= itemDropPercentage)
            {
                int tmp = Random.Range(0, items.Count);
                inventory.SetAddItemPanel(items[tmp]);
                items.RemoveAt(tmp);
            }
        }
        else if (player.health.isDead)
        {

        }
        else
        {
            isPlayerTurn = !isPlayerTurn;
            player.canDoAction = isPlayerTurn;
            enemy.canDoAction = !isPlayerTurn;

            if (!isPlayerTurn)
            {
                for (int i = 0; i < playerButtons.Length; i++)
                {
                    playerButtons[i].interactable = false;
                    playerButtons[i].transform.localScale = Vector3.one;
                    playerInteractiveButtons[i].enabled = false;
                }
            }

            yield return new WaitForSeconds(1.5f);

            if (isPlayerTurn)
            {
                for (int i = 0; i < playerButtons.Length; i++)
                {
                    playerButtons[i].interactable = true;
                    playerInteractiveButtons[i].enabled = true;
                }
            }

            if (!isPlayerTurn)
            {
                switch (enemyAI.SelectAction())
                {
                    case 1:
                        enemy.Attack();
                        break;

                    case 2:
                        enemy.Defend();
                        break;

                    case 3:
                        enemy.Focus();
                        break;

                    case 4:
                        enemy.Heal();
                        break;
                }

            }
        }
    }

    void CreateNewEnemy()
    {
        // Get random enemy
        int currentEnemyId = Random.Range(0, enemysPrefabs.Count);
        enemy = Instantiate(enemysPrefabs[currentEnemyId], enemyPos, Quaternion.identity).GetComponent<CharacterController>();
        enemysPrefabs.RemoveAt(currentEnemyId);
        // Set enemy stats
        enemy.loopManager = this;
        enemy.statBar = enemyStatBar;
        enemy.health.statBar = enemyStatBar;
        enemyStatBar.SetHealthVisual(enemy.health.maxHealth, enemy.health.maxHealth);
        enemyStatBar.SetShieldVisual(enemy.defensesPointBonnus);
        enemyStatBar.SetAttackVisual(enemy.attackPointBonnus);

        // Set enemy AI
        enemyAI.controller = enemy;
        enemyAI.playerController = player;

        enemy.enemy = player;
    }
}