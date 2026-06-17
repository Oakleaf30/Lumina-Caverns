using UnityEngine;

public class Station : MonoBehaviour
{
    [SerializeField] private GameEvent onStationInteracted;

    public void Interact()
    {
        onStationInteracted.Raise();
    }
}