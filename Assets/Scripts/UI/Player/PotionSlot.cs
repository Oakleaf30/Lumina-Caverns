using TMPro;
using UnityEngine;

public class PotionSlot : MonoBehaviour
{
    [SerializeField] private GameEvent onPotionCountChanged;

    [SerializeField] private TextMeshProUGUI text;

    private RunState RunState => GameSession.Instance.runState;

    private void OnEnable()
    {
        onPotionCountChanged.Subscribe(UpdateText);
    }

    private void OnDisable()
    {
        onPotionCountChanged.Unsubscribe(UpdateText);
    }

    private void UpdateText()
    {
        text.text = RunState.potionCount.ToString();
    }
}
