using TMPro;
using UnityEngine;

public class BombSlot : MonoBehaviour
{
    [SerializeField] private GameEvent onBombCountChanged;

    [SerializeField] private TextMeshProUGUI text;

    private RunState RunState => GameSession.Instance.runState;

    private void OnEnable()
    {
        onBombCountChanged.Subscribe(UpdateText);
    }

    private void OnDisable()
    {
        onBombCountChanged.Unsubscribe(UpdateText);
    }

    private void UpdateText()
    {
        text.text = RunState.bombCount.ToString();
    }
}
