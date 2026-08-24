using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private RunState RunState => GameSession.Instance.runState;

    private ItemData item;
    private string header;
    private string body;

    private void Start()
    {
        var slot = GetComponent<InventorySlotUI>();
        item = slot.item;

        if (item is PickaxeData pickaxe)
        {
            AdaptPickaxe();
        } else if (item is EquipmentData equipment)
        {
            switch (equipment.category)
            {
                case ItemCategory.Sword:
                    AdaptSword();
                    break;

                case ItemCategory.Armour:
                    AdaptArmour();
                    break;
            }
        }
        else
            AdaptItem();
    }

    private void AdaptPickaxe()
    {
        header = $"{RunState.tier.tierName} {item.displayName}";
        body = $"Damage: {RunState.pickaxe.damage}\n" +
               $"Durability repaired per bar: {RunState.durabilityPerBar}";
    }

    private void AdaptSword()
    {
        header = $"{item.displayName}";
        body = $"Damage: {RunState.sword.damage}";
    }

    private void AdaptArmour()
    {
        header = $"{item.displayName}";
    }

    private void AdaptItem()
    {
        header = $"{item.displayName}";

        switch (item.itemId)
        {
            case "potion":
                body = "Restores 30% max health over 10 seconds\n" +
                                "Max carry capacity of 5\n" +
                                "Press 1 to drink";
                break;
            case "bomb":
                body = "Destroys nodes in a small radius\n" +
                                "Max carry capacity of 3\n" +
                                "Press 2 to drink";
                break;
            case "amulet":
                body = "Prevents you from losing items when you die\n" +
                                "Breaks upon use\n";
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TooltipManager.Instance.Show(header, body, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }
}