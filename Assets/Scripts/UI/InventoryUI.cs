using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private Dictionary<ItemCategory, Transform> categorySections;
    private Dictionary<ItemCategory, List<GameObject>> sectionPools = new Dictionary<ItemCategory, List<GameObject>>();

    void Awake()
    {
        playerInventory = GameObject.FindWithTag("Player").GetComponent<PlayerInventory>();

        categorySections = new Dictionary<ItemCategory, Transform>
    {
        { ItemCategory.Ore, oreSection },
        { ItemCategory.Gem, gemSection },
        { ItemCategory.MonsterDrop, dropSection },
        { ItemCategory.Misc, miscSection }
    };

        foreach (var category in categorySections.Keys)
            sectionPools[category] = new List<GameObject>();
    }

    protected override void OpenMenu()
    {
        base.OpenMenu();

        RefreshUI();
    }

    void RefreshUI()
    {
        ItemContainer targetContainer = SceneManager.GetActiveScene().name == "Base" ? BaseStorage.Instance : playerInventory;

        foreach (var kv in categorySections)
        {
            var slots = targetContainer.GetItemsByCategory(kv.Key);
            BuildSection(kv.Value, slots, sectionPools[kv.Key]);
        }
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