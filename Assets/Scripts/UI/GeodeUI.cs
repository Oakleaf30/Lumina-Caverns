using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GeodeUI : StationUI
{
    [SerializeField] private GeodeData geode;
    [SerializeField] private InventorySlotUI slotG1;
    [SerializeField] private Button button;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform lootContainer;
    [SerializeField] private float lootInterval = 0.3f;

    BaseStorage Storage => BaseStorage.Current;
    private int GeodeAmount => Storage.GetQuantity(geode);

    protected override void OpenMenu()
    {
        base.OpenMenu();
        UpdateGeodeDisplay();
    }

    public void UpdateGeodeDisplay()
    {
        slotG1.Set(geode, GeodeAmount);

        button.interactable = GeodeAmount == 0 ? false : true;
    }

    public void ButtonClicked()
    {
        StartCoroutine(OpenGeodes());
    }

    private IEnumerator OpenGeodes()
    {
        for (int i = 0; i < GeodeAmount; i++)
        {
            var loot = geode.lootTable.GetRandomLoot();
            GameObject slot = Instantiate(slotPrefab, lootContainer);
            slot.GetComponent<InventorySlotUI>().Set(loot.item, loot.amount);
            Storage.AddItem(loot.item, loot.amount);

            yield return new WaitForSeconds(lootInterval);
        }

        Storage.RemoveItem(geode, GeodeAmount);
    }
}
