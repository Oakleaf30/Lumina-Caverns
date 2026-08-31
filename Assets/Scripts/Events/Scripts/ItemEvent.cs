using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewItemEvent", menuName = "Events/Item Event")]
public class ItemEvent : ScriptableObject
{
    private UnityAction<ItemData> onEventRaised;

    public void Raise(ItemData item)
    {
        onEventRaised?.Invoke(item);
    }

    public void Subscribe(UnityAction<ItemData> action)
    {
        onEventRaised += action;
    }

    public void Unsubscribe(UnityAction<ItemData> action)
    {
        onEventRaised -= action;
    }
}