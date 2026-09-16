using UnityEngine;

public class PlayerEnchants : MonoBehaviour
{
    [SerializeField] private GameEvent onBaseReturn;

    private void OnEnable() => onBaseReturn.Subscribe(OnBaseReturn);
    private void OnDisable() => onBaseReturn.Unsubscribe(OnBaseReturn);

    private void OnBaseReturn()
    {
        GameSession.Instance.runState.OnRunEnded();
    }
}