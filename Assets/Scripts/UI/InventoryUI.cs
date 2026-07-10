using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : StationUI
{
    private PlayerInventory playerInventory;
    [SerializeField] private GameObject slotPrefab;

    [SerializeField] private Transform oreSection;
    [SerializeField] private Transform gemSection;
    [SerializeField] private Transform dropSection;
    [SerializeField] private Transform miscSection;

    private List<GameObject> oreSlotObjects = new List<GameObject>();
    private List<GameObject> gemSlotObjects = new List<GameObject>();
    private List<GameObject> dropSlotObjects = new List<GameObject>();
    private List<GameObject> miscSlotObjects = new List<GameObject>();

    private void Awake()
    {
        playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();
    }

    protected override void OpenMenu()
    {
        base.OpenMenu();

        RefreshUI();
    }

    void RefreshUI()
    {
        var oreSlots = playerInventory.GetItemsByCategory(ItemCategory.Ore);
        var gemSlots = playerInventory.GetItemsByCategory(ItemCategory.Gem);
        var dropSlots = playerInventory.GetItemsByCategory(ItemCategory.MonsterDrop);
        var miscSlots = playerInventory.GetItemsByCategory(ItemCategory.Misc);

        BuildSection(oreSection, oreSlots, oreSlotObjects);
        BuildSection(gemSection, gemSlots, gemSlotObjects);
        BuildSection(dropSection, dropSlots, dropSlotObjects);
        BuildSection(miscSection, miscSlots, miscSlotObjects);
    }

    void BuildSection(Transform sectionParent, List<InventorySlot> slots, List<GameObject> slotObjects)
    {
        // hide every slot object currently in the pool
        foreach (var slotObj in slotObjects)
            slotObj.SetActive(false);

        // reuse existing slot objects, or make new ones if we need more
        for (int i = 0; i < slots.Count; i++)
        {
            GameObject slotObj;

            if (i < slotObjects.Count)
            {
                slotObj = slotObjects[i];
            }
            else
            {
                slotObj = Instantiate(slotPrefab, sectionParent);
                slotObjects.Add(slotObj);
            }

            slotObj.SetActive(true);

            slotObj.GetComponent<InventorySlotUI>().Set(slots[i].item, slots[i].quantity);
        }
    }
}