using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoopManager : MonoBehaviour
{
    public GameObject playerGO;

    // Enemy stats
    [SerializeField] List<GameObject> enemysPrefabs = new List<GameObject>();
    List<GameObject> enemyToSpawn = new List<GameObject>();
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

        StartCoroutine(CreateNewEnemy());
    }

    public IEnumerator NextTurn()
    {
        if (enemy.health.isDead)
        {
            // Close player button
            for (int i = 0; i < playerButtons.Length; i++)
            {
                playerButtons[i].interactable = false;
                playerButtons[i].transform.localScale = Vector3.one;
                playerInteractiveButtons[i].enabled = false;
            }

            isPlayerTurn = false;

            scoreManager.AddScore(5);
            if(Random.Range(0,100) <= itemDropPercentage)
            {
                yield return new WaitForSeconds(1f);

                int tmp = Random.Range(0, items.Count);
                inventory.SetAddItemPanel(items[tmp]);
                //items.RemoveAt(tmp);
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

            yield return new WaitForSeconds(1.2f);

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

    public IEnumerator CreateNewEnemy()
    {
        if (enemyToSpawn.Count <= 0)
        {
            print("Reload enemys list");
            enemyToSpawn = new List<GameObject>(enemysPrefabs);
        }

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


        yield return new WaitForSeconds(1f);

        StartCoroutine(NextTurn());
    }
}