using UnityEngine;

public class FurnaceUI : StationUI
{
    protected override void OpenMenu()
    {
        // 1. Run the base code first (Sets panel active, pauses Time.timeScale)
        base.OpenMenu();

        // 2. Run your unique Anvil logic
        UpdateFurnaceDisplay();
    }

    private void UpdateFurnaceDisplay()
    {
        Debug.Log("Checking player inventory for Coal...");
        // Code to populate upgrades or tool repair costs goes here
    }
}