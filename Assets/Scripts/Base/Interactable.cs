using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private GameEvent onInteracted;

    public void Interact()
    {
        onInteracted.Raise();
    }
}