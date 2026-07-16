using UnityEngine;

public class BaseLadder : MonoBehaviour
{
    [SerializeField] private GameEvent onLadderUsed;

    private void OnEnable()
    {
        onLadderUsed.Subscribe(LoadMines);
    }

    private void OnDisable()
    {
        onLadderUsed.Unsubscribe(LoadMines);
    }

    private void LoadMines()
    {
        ScreenFader.Instance.TransitionToScene("Mine");
    }
}
