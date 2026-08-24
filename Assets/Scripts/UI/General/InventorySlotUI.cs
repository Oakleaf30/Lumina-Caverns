using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SlotDisplayMode
{
    Resource,
    Equipment
}

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;
    public ItemData item;

    public void Set(ItemData item, int quantity, SlotDisplayMode mode = SlotDisplayMode.Resource)
    {
        this.item = item;
        icon.sprite = item.icon;
        quantityText.text = mode == SlotDisplayMode.Resource
            ? quantity.ToString()
            : (quantity == 0 ? "" : quantity.ToString());
    }

    public void Clear()
    {
        icon.sprite = null;
        quantityText.text = "";
    }
}