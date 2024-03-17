using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

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

            // Set item drop
            if (Random.Range(0, 100) <= itemDropPercentage)
            {
                List<ItemData> comsumablesItems = new List<ItemData>(items.Where(elem => elem.equipmentType == EquipmentType.Comsumable));
                List<ItemData> equipmentsItems = new List<ItemData>(items.Where(elem => elem.equipmentType != EquipmentType.Comsumable));

                if (Random.Range(0, 100) < 40)
                {
                    inventory.SetAddItemPanel(equipmentsItems.GetRandom());
                    //print("equipment slected");
                }
                else
                {
                    inventory.SetAddItemPanel(comsumablesItems.GetRandom());
                    //print("Comsumable selected");
                }

                //items.RemoveAt(tmp);
            }
            else StartCoroutine(CreateNewEnemy());
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
                enemyAI.SelectAction();
            }
        }
    }
     
    public IEnumerator CreateNewEnemy()
    {
        if (enemy != null) Destroy(enemy.gameObject);
        if (enemyToSpawn.Count <= 0) enemyToSpawn = new List<GameObject>(enemysPrefabs);

        // Get random enemy
        int currentEnemyId = Random.Range(0, enemyToSpawn.Count);
        enemy = Instantiate(enemyToSpawn[currentEnemyId], new Vector2(10, 0), Quaternion.identity).GetComponent<CharacterController>();
        enemyToSpawn.RemoveAt(currentEnemyId);
        enemy.transform.DOMove(enemyPos, 1.5f);

        // Set equipmentItem
        List<ItemData> equipmentsItems = new List<ItemData>(items.Where(elem => elem.equipmentType != EquipmentType.Comsumable));
        int equipmentProbability = scoreManager.score;
        bool haveItem = false;
        do
        {
            int tmp = Random.Range(0, 100);
            if (tmp < equipmentProbability)
            {
                equipmentProbability -= tmp;
                haveItem = true;
                ItemData currentItem = equipmentsItems.GetRandom();
                enemy.equipmentAttackPoint += currentItem.attackPointGiven;
                enemy.equipmentDefensePoint += currentItem.defensePointGiven;
                enemy.equipmentHealthPoint += currentItem.healthPointGiven;
            }
            else haveItem = false;
        } while (haveItem);

        // Set consumable item
        int itemProbability;
        do
        {
            itemProbability = Random.Range(0, 100);
            if(itemProbability < 30)
            {
                int tmp = Random.Range(1, 4);
                if (tmp == 1) enemy.healPotionCount++;
                else if (tmp == 2) enemy.defensePotionCount++;
                else enemy.attackPotionCount++;
            }
        }
        while (itemProbability < 30);
        
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