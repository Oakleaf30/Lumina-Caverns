using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeodeUI : StationUI
{
    [SerializeField] private GeodeData geode;
    [SerializeField] private InventorySlotUI slotG1;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform lootContainer;
    [SerializeField] private float lootInterval = 0.3f;

    BaseStorage Storage => BaseStorage.Current;
    private int GeodeAmount => Storage.GetQuantity(geode);
    private int GeodePity
    {
        get => GameSession.Instance.runState.geodePity;
        set => GameSession.Instance.runState.geodePity = value;
    }

    private bool showingOpen = true;

    protected override void OpenMenu()
    {
        base.OpenMenu();
        UpdateGeodeDisplay();
    }

    public void UpdateGeodeDisplay()
    {
        slotG1.Set(geode, GeodeAmount);

        button.interactable = GeodeAmount == 0 && showingOpen ? false : true;
    }

    public void ButtonClicked()
    {
        if (showingOpen)
        {
            StartCoroutine(OpenGeodes());
            SetButtonState(false);
        }
        else
        {
            ClaimLoot();
            SetButtonState(true);
        }
    }

    private void SetButtonState(bool opening)
    {
        showingOpen = opening;
        buttonText.text = opening ? "Open" : "Claim";
    }

    private IEnumerator OpenGeodes()
    {
        button.interactable = false;

        for (int i = 0; i < GeodeAmount; i++)
        {
            var loot = geode.lootTable.GetRandomLoot();
            loot = CheckPity(loot);
            GameObject slot = Instantiate(slotPrefab, lootContainer);
            slot.GetComponent<InventorySlotUI>().Set(loot.item, loot.amount);
            Storage.AddItem(loot.item, loot.amount);

            yield return new WaitForSeconds(lootInterval);
        }

        SetButtonState(false);
        Storage.RemoveItem(geode, GeodeAmount);
        UpdateGeodeDisplay();
    }

    private void ClaimLoot()
    {
        SetButtonState(true);
        UpdateGeodeDisplay();
        for (int i = lootContainer.childCount - 1; i >= 0; i--)
        {
            Destroy(lootContainer.GetChild(i).gameObject);
        }
    }

    private ItemAmount CheckPity(ItemAmount loot)
    {
        GeodePity = loot.item != geode.lootTable.pityTargetItem ? GeodePity + 1 : 0;

        if (GeodePity < geode.lootTable.pityThreshold)
            return loot;

        GeodePity = 0;
        return geode.lootTable.pityReward;
    }
}
