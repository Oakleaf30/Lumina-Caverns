using UnityEngine;

public class LadderTile : MonoBehaviour
{
    [SerializeField] private GameEvent OnLadderUsed;
    [SerializeField] private GameObject ladderPrefab;

    [SerializeField] private Vector3Event onLadderCheck;
    private int numberOfNodes = 0;
    private float ladderChance;
    private float baseLadderChance = 0.02f;

    private float numberOfEnemies = 0;
    [SerializeField] private Vector3Event onEnemyLadder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        OnLadderUsed.Subscribe(LoadMines);
        onLadderCheck.Subscribe(CheckLadderChance);
        onEnemyLadder.Subscribe(EnemyLadderCheck);
    }

    private void OnDisable()
    {
        OnLadderUsed.Unsubscribe(LoadMines);
        onLadderCheck.Unsubscribe(CheckLadderChance);
    }

    private void LoadMines()
    {
        ScreenFader.Instance.TransitionToScene("Mine");
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
        if (Random.value < 0.15f)
        {
            SpawnLadder(enemyPos);
        }

        CheckNoEnemies();
    }

    private void CheckNoEnemies()
    {
        numberOfEnemies--;
        if (numberOfEnemies == 0) baseLadderChance += 0.04f;
    }

    private void SpawnLadder(Vector3 position)
    {
        Instantiate(ladderPrefab, position, Quaternion.identity);
    }
}

