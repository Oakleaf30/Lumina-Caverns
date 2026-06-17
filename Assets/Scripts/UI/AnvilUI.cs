using UnityEngine;

public class AnvilUI : StationUI
{
    protected override void OpenMenu()
    {
        // 1. Run the base code first (Sets panel active, pauses Time.timeScale)
        base.OpenMenu();

        // 2. Run your unique Anvil logic
        UpdateAnvilDisplay();
    }

    private void UpdateAnvilDisplay()
    {
        Debug.Log("Checking player inventory for Geodes...");
        // Code to populate upgrades or tool repair costs goes here
    }
}