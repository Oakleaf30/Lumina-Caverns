using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LadderTile : MonoBehaviour
{
    [SerializeField] private GameEvent onLadderUsed;
    [SerializeField] private GameObject ladderPrefab;

    [SerializeField] private Vector3Event onLadderCheck;
    private int numberOfNodes = 0;
    private float ladderChance;
    [SerializeField] private float baseLadderChance = 0.02f;

    private float numberOfEnemies = 0;
    [SerializeField] private Vector3Event onEnemyLadder;

    [SerializeField] private GameEvent onShaftUsed;
    [SerializeField] private GameObject shaftPrefab;
    [SerializeField] private float defaultShaftChance;
    private float shaftChance;

    private BiomeData activeBiome;
    [SerializeField] private BiomeData coalFloor;
    [SerializeField] private BiomeData infestedFloor;

    [SerializeField] private GameEvent onLastEnemyDefeated;

    [SerializeField] LevelGenerator level;

    private void Start()
    {
        activeBiome = level.biomeData;

        if (activeBiome.biomeName == "Coal" || activeBiome.biomeName == "Infested")
        {
            shaftChance = 0;
        } else
        {
            shaftChance = defaultShaftChance;
        }
    }
    private void OnEnable()
    {
        onLadderUsed.Subscribe(LoadMines);
        onLadderCheck.Subscribe(CheckLadderChance);
        onEnemyLadder.Subscribe(EnemyLadderCheck);
        onShaftUsed.Subscribe(JumpShaft);
    }

    private void OnDisable()
    {
        onLadderUsed.Unsubscribe(LoadMines);
        onLadderCheck.Unsubscribe(CheckLadderChance);
        onEnemyLadder.Unsubscribe(EnemyLadderCheck);
        onShaftUsed.Unsubscribe(JumpShaft);
    }

    private void LoadMines()
    {
        ScreenFader.Instance.TransitionToScene("Mine");
        GameSession.Instance.runState.currentFloor++;
    }

    public void AddNodes(int amount)
    {
        numberOfNodes += amount;
        CalculateLadderChance();
    }

    private void CalculateLadderChance()
    {
        ladderChance = Mathf.Min(1f, baseLadderChance + (1f / numberOfNodes));
    }

    private void CheckLadderChance(Vector3 nodePos)
    {
        CalculateLadderChance();

        if (Random.value < ladderChance)
        {
            SpawnLadder(nodePos);
        } else
        {
            numberOfNodes--;
        }
    }

    public void AddEnemies(int amount)
    {
        numberOfEnemies += amount;
    }

    private void EnemyLadderCheck(Vector3 enemyPos)
    {
        if (Random.value < 0.15f && activeBiome.biomeName != "Infested")
        {
            SpawnLadder(enemyPos);
        }

        CheckNoEnemies(enemyPos);
    }

    private void CheckNoEnemies(Vector3 enemyPos)
    {
        numberOfEnemies--;
        if (numberOfEnemies == 0)
        {
            if (activeBiome.biomeName == "Infested")
            {
                transform.position = enemyPos;
                SpawnLadder(enemyPos);
                onLastEnemyDefeated.Raise();

                GameObject chest = Instantiate(chestPrefab, closestAnchor, Quaternion.identity);
                chest.GetComponent<Chest>().InitialiseImmediate(smallChest);
            }
            else
            {
                baseLadderChance += 0.04f;
            }
        }
    }

    private void SpawnLadder(Vector3 position)
    {
        GameObject prefab = Random.value < shaftChance ? shaftPrefab : ladderPrefab;
        Instantiate(prefab, position, Quaternion.identity);
    }

    private void JumpShaft()
    {
        GameSession.Instance.runState.currentFloor++;

        BiomeData nextBiome = Random.value < 0.5f ? coalFloor : infestedFloor;

        TransitionState.FloorTransition(activeBiome, nextBiome, SceneManager.GetActiveScene().name);
    }

    [Header("Chest References")]
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private ChestData smallChest;

    private Vector3 closestAnchor;
    private float closestDist = float.MaxValue;

    public void CompareDistances(Vector3 anchorPos)
    {
        float sqrDist = (anchorPos - transform.position).sqrMagnitude;
        if (sqrDist < closestDist)
        {
            closestDist = sqrDist;
            closestAnchor = anchorPos;
        }
    }
}

