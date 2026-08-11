using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeodeUI : TabPanelUI
{
    [Header("UI Settings")]
    [SerializeField] private float lootInterval = 0.3f;

    [Header("UI References")]
    [SerializeField] private InventorySlotUI slotG1;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform lootContainer;

    [Header("References")]
    [SerializeField] private GeodeData geode;
    

    BaseStorage Storage => BaseStorage.Current;
    private int GeodeAmount => Storage.GetQuantity(geode);
    private int GeodePity
    {
        get => GameSession.Instance.runState.geodePity;
        set => GameSession.Instance.runState.geodePity = value;
    }

    private bool showingOpen = true;

    public override void UpdateDisplay()
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

            yield return new WaitForSecondsRealtime(lootInterval);
        }

        SetButtonState(false);
        Storage.RemoveItem(geode, GeodeAmount);
        UpdateDisplay();
    }

    private void ClaimLoot()
    {
        SetButtonState(true);
        UpdateDisplay();
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
