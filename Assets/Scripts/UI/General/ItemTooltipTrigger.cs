using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private RunState RunState => GameSession.Instance.runState;

    private ItemData item;
    private string header;
    private string body;

    private void AdaptPickaxe()
    {
        header = $"{RunState.tier.tierName} {item.displayName}";
        body = $"Damage: {RunState.pickaxe.damage}\n" +
               $"Durability repaired per bar: {RunState.durabilityPerBar}";

        if (RunState.tierIndex == 2)
        {
            body += $"\nAbility: {RunState.pickaxe.specialAbility.description}";
        }
    }

    private void AdaptSword()
    {
        header = item.displayName;
        body = $"Damage: {RunState.sword.damage}";
    }

    private void AdaptArmour()
    {
        header = item.displayName;
        body = RunState.armour.maxHealth.ToString();
    }

    private void AdaptItem()
    {
        header = $"{item.displayName}";
        body = item.info;
    }

    private void AdaptPopup()
    {
        var slot = GetComponent<InventorySlotUI>();
        item = slot.item;

        if (item is PickaxeData pickaxe)
        {
            AdaptPickaxe();
        }
        else if (item is EquipmentData equipment)
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        AdaptPopup();
        TooltipManager.Instance.Show(header, body, transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipManager.Instance.Hide();
    }
}