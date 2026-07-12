using System;
using UnityEngine;

public class LadderTile : MonoBehaviour
{
    [SerializeField] private GameEvent OnLadderUsed;
    [SerializeField] private GameObject ladderPrefab;

    [SerializeField] private Vector3Event onNodeBreak;
    private int numberOfNodes = 0;
    private float ladderChance;

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
        onNodeBreak.Subscribe(CheckLadderChance);
    }

    private void OnDisable()
    {
        OnLadderUsed.Unsubscribe(LoadMines);
        onNodeBreak.Unsubscribe(CheckLadderChance);
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
        ladderChance = MathF.Min(1f, 0.02f + (1f / numberOfNodes));
        Debug.Log(numberOfNodes);
        Debug.Log(ladderChance);
    }

    private void CheckLadderChance(Vector3 nodeLocation)
    {
        CalculateLadderChance();

        if (UnityEngine.Random.value < ladderChance)
        {
            SpawnLadder(nodeLocation);
        } else
        {
            numberOfNodes--;
        }
    }

    private void SpawnLadder(Vector3 position)
    {
        Instantiate(ladderPrefab, position, Quaternion.identity);
        Debug.Log("Spawned");
    }
}

