using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Events/Game Event")]
public class GameEvent : ScriptableObject
{
    private UnityAction onEventRaised;

    public void Raise()
    {
        onEventRaised?.Invoke();
    }

    public void Subscribe(UnityAction action)
    {
        onEventRaised += action;
    }

    public void Unsubscribe(UnityAction action)
    {
        onEventRaised -= action;
    }
}