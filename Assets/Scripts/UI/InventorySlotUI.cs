using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI quantityText;

    public void Set(ItemData item, int quantity)
    {
        icon.sprite = item.icon;
        quantityText.text = quantity == 0 ? "" : quantity.ToString();
    }
}