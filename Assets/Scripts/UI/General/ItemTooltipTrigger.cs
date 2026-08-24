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

        if (item is EquipmentData equipment)
        {
            switch (equipment.type)
            {
                case EquipmentCategory.Pickaxe:
                    AdaptPickaxe();
                    break;
                case EquipmentCategory.Sword:
                    AdaptSword();
                    break;

                case EquipmentCategory.Armour:
                    AdaptArmour();
                    break;
            }
        } else
            AdaptItem();
    }

    private void AdaptPickaxe()
    {
        header = $"{RunState.tier.tierName} {item.displayName}";
    }

    private void AdaptSword()
    {

    }

    private void AdaptArmour()
    {

    }

    private void AdaptItem()
    {

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