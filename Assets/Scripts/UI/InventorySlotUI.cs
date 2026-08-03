using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;

    public void Set(ItemData item, int quantity, SlotDisplayMode mode = SlotDisplayMode.Resource)
    {
        icon.sprite = item.icon;
        quantityText.text = mode == SlotDisplayMode.Resource
            ? quantity.ToString()
            : (quantity == 0 ? "" : quantity.ToString());
    }
}