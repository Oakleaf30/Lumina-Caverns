using TMPro;
using UnityEngine;

public class DurabilityUI : MonoBehaviour
{
    [SerializeField] private GameEvent onDurabilityChanged;

    [SerializeField] private TextMeshProUGUI text;

    private RunState RunState => GameSession.Instance.runState;

    private void OnEnable()
    {
        onDurabilityChanged.Subscribe(UpdatePickaxeText);
    }

    private void OnDisable()
    {
        onDurabilityChanged.Unsubscribe(UpdatePickaxeText);
    }

    private void UpdatePickaxeText()
    {
        text.text = $"Pickaxe: {RunState.pickaxeDurability}/{RunState.tier.maxDurability}";
    }
}
