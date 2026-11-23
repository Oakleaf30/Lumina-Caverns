using TMPro;
using UnityEngine;

public class DungeonCounterUI : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI counter;

    [SerializeField] private GameEvent onDungeonEntered;
    [SerializeField] private GameEvent onEnemyDeath;

    private int totalEnemies;
    private int enemiesRemaining;

    private void OnEnable()
    {
        onDungeonEntered.Subscribe(Show);
        onEnemyDeath.Subscribe(SubtractEnemy);
    }

    private void OnDisable()
    {
        onDungeonEntered.Unsubscribe(Show);
        onEnemyDeath?.Unsubscribe(SubtractEnemy);
    }

    private void Show()
    {
        panel.SetActive(true);
    }

    public void AddEnemies(int enemies)
    {
        totalEnemies += enemies;
        enemiesRemaining += enemies;
        counter.text = $"{enemiesRemaining}/{totalEnemies}";
    }

    private void SubtractEnemy()
    {
        enemiesRemaining--;

        if (enemiesRemaining == 0)
            panel.SetActive(false);
        else
            counter.text = $"{enemiesRemaining}/{totalEnemies}";
    }
}
