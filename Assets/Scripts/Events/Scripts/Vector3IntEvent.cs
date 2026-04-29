using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewVector3IntEvent", menuName = "Events/Vector3Int Event")]
public class Vector3IntEvent : ScriptableObject
{
    // The UnityAction now carries a Vector3Int payload
    private UnityAction<Vector3Int> onEventRaised;

    // The Raise method MUST accept the Vector3Int parameter
    public void Raise(Vector3Int value)
    {
        onEventRaised?.Invoke(value);
    }

    // The Subscribe method MUST accept a method that takes a Vector3Int parameter
    public void Subscribe(UnityAction<Vector3Int> action)
    {
        onEventRaised += action;
    }

    // The Unsubscribe method also uses the parameterized action
    public void Unsubscribe(UnityAction<Vector3Int> action)
    {
        onEventRaised -= action;
    }
}