using TMPro;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private GameEvent onHealthChanged;

    [SerializeField] private TextMeshProUGUI text;

    private RunState RunState => GameSession.Instance.runState;

    private void OnEnable()
    {
        onHealthChanged.Subscribe(UpdateHealth);
    }

    private void OnDisable()
    {
        onHealthChanged.Unsubscribe(UpdateHealth);
    }

    private void UpdateHealth()
    {
        text.text = $"Health: {RunState.currentHealth}/{RunState.armour.maxHealth}";
    }
}
