using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoopManager : MonoBehaviour
{
    public GameObject playerGO;
    bool isPlayerTurn = false;

    [Header("Enemy references")]
    [SerializeField] List<GameObject> enemysPrefabs = new List<GameObject>();
    List<GameObject> enemyToSpawn = new List<GameObject>();
    EnemyAI enemyAI;
    public Vector2 enemyPos;

    // Stats bar
    [HideInInspector] public CharacterController player, enemy;
    public StatsBar playerStatBar, enemyStatBar;

    [Header("Buttons references")]
    public Button[] playerButtons;
    List<InteractiveButton> playerInteractiveButtons = new List<InteractiveButton>();

    [HideInInspector] public int waveCount, enemysKilled, damagedInflicted, damagedReceived, damagedBlocked, lifeRecorvery;

    // Items
    public int itemDropPercentage;
    public List<ItemData> items = new List<ItemData>();
    
    ScoreManager scoreManager;
    GameOverManager deathManager;
    Inventory inventory;

    private void Awake()
    {
        scoreManager = GetComponent<ScoreManager>();
        deathManager = GetComponent<GameOverManager>();
        inventory = GetComponent<Inventory>();
        inventory.loopManager = this;

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
        player.SetStatBar();
        inventory.playerController = player;

        enemyAI = new EnemyAI();

        // unable player button
        for (int i = 0; i < playerButtons.Length; i++)
        {
            playerButtons[i].interactable = false;
            playerButtons[i].transform.localScale = Vector3.one;
            playerInteractiveButtons[i].enabled = false;
        }

        StartCoroutine(CreateNewEnemy());
    }

    public IEnumerator NextTurn()
    {
        // Close player button
        for (int i = 0; i < playerButtons.Length; i++)
        {
            playerButtons[i].interactable = false;
            playerButtons[i].transform.localScale = Vector3.one;
            playerInteractiveButtons[i].enabled = false;
        }

        yield return new WaitForSeconds(1.2f);

        if (enemy.health.isDead)
        {
            isPlayerTurn = false;

            enemysKilled++;
            scoreManager.AddScore(5);
         
            if(Random.Range(0,100) <= itemDropPercentage)
            {
                int tmp = Random.Range(0, items.Count);
                inventory.SetAddItemPanel(items[tmp]);
                //items.RemoveAt(tmp);
            }
        }
        else if (player.health.isDead)
        {
            if (!inventory.isInventoryOpon) inventory.InventoryButton();
            deathManager.SetTextInfos(scoreManager.score, waveCount, enemysKilled, damagedInflicted, damagedReceived, damagedBlocked, lifeRecorvery);
            deathManager.OpenDeathPanel();
        }
        else
        {
            isPlayerTurn = !isPlayerTurn;
            player.canDoAction = isPlayerTurn;
            enemy.canDoAction = !isPlayerTurn;

            if (!isPlayerTurn)
            {
                // unable player button
                for (int i = 0; i < playerButtons.Length; i++)
                {
                    playerButtons[i].interactable = false;
                    playerButtons[i].transform.localScale = Vector3.one;
                    playerInteractiveButtons[i].enabled = false;
                }
            }

            if (isPlayerTurn)
            {
                for (int i = 0; i < playerButtons.Length; i++)
                {
                    playerButtons[i].interactable = true;
                    playerInteractiveButtons[i].enabled = true;
                }
            }
            else
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
     
    public IEnumerator CreateNewEnemy()
    {
        if (enemyToSpawn.Count <= 0) enemyToSpawn = new List<GameObject>(enemysPrefabs);

        // Get random enemy
        int currentEnemyId = Random.Range(0, enemyToSpawn.Count);
        enemy = Instantiate(enemyToSpawn[currentEnemyId], new Vector2(10, 0), Quaternion.identity).GetComponent<CharacterController>();
        enemyToSpawn.RemoveAt(currentEnemyId);
        enemy.transform.DOMove(enemyPos, 1.5f);
        // Set enemy stats
        enemy.loopManager = this;
        enemy.statBar = enemyStatBar;
        enemy.health.statBar = enemyStatBar;
        enemy.SetStatBar();

        // Set enemy AI
        enemyAI.controller = enemy;
        enemyAI.playerController = player;

        enemy.enemy = player;
        player.enemy = enemy;

        waveCount++;

        yield return new WaitForSeconds(1f);

        StartCoroutine(NextTurn());
    }
}