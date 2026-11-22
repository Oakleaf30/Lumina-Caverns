using UnityEngine;

public class DungeonEntrance : MonoBehaviour
{
    [SerializeField] private GameEvent onDungeonCleared;

    private void OnEnable()
    {
        onDungeonCleared.Subscribe(OpenDoor);
    }

    private void OnDisable()
    {
        onDungeonCleared.Unsubscribe(OpenDoor);
    }

    private void OpenDoor()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetBool("IsOpen", true);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
