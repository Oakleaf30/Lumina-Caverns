using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareItemUI : MonoBehaviour
{
    [SerializeField] private ItemEvent onItemCollected;
    [SerializeField] private InventorySlotUI popupPrefab;
    [SerializeField] private Transform popupContainer;
    [SerializeField] private float displayDuration = 3f;

    private class ActivePopup
    {
        public InventorySlotUI instance;
        public int count;
        public Coroutine closeRoutine;
    }

    private Dictionary<ItemData, ActivePopup> activePopups = new();

    private void OnEnable()
    {
        onItemCollected.Subscribe(CheckRarity);
    }

    private void OnDisable()
    {
        onItemCollected.Unsubscribe(CheckRarity);
    }

    private void CheckRarity(ItemData item)
    {
        if (item.category == ItemCategory.Gem)
        {
            if (activePopups.TryGetValue(item, out var popup))
            {
                // Already showing this item — just bump the count and reset the timer
                popup.count++;
                popup.instance.Set(item, popup.count);

                StopCoroutine(popup.closeRoutine);
                popup.closeRoutine = StartCoroutine(CloseAfterDelay(item));
            }
            else
            {
                // First pickup of this item — spawn a new popup
                var instance = Instantiate(popupPrefab, popupContainer);
                instance.Set(item, 0, SlotDisplayMode.Equipment);

                var newPopup = new ActivePopup { instance = instance, count = 1 };
                newPopup.closeRoutine = StartCoroutine(CloseAfterDelay(item));
                activePopups[item] = newPopup;
            }
        }
    }

    private IEnumerator CloseAfterDelay(ItemData item)
    {
        yield return new WaitForSeconds(displayDuration);

        if (activePopups.TryGetValue(item, out var popup))
        {
            Destroy(popup.instance.gameObject);
            activePopups.Remove(item);
        }
    }
}
