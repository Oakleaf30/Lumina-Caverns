using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewVector3Event", menuName = "Events/Vector3 Event")]
public class Vector3Event : ScriptableObject
{
    private UnityAction<Vector3> onEventRaised;

    public void Raise(Vector3 position)
    {
        onEventRaised?.Invoke(position);
    }

    public void Subscribe(UnityAction<Vector3> action)
    {
        onEventRaised += action;
    }

    public void Unsubscribe(UnityAction<Vector3> action)
    {
        onEventRaised -= action;
    }
}