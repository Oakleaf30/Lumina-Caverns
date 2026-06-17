using UnityEngine;

public class WorkbenchUI : StationUI
{
    protected override void OpenMenu()
    {
        // 1. Run the base code first (Sets panel active, pauses Time.timeScale)
        base.OpenMenu();

        // 2. Run your unique Anvil logic
        UpdateWorkbenchDisplay();
    }

    private void UpdateWorkbenchDisplay()
    {
        Debug.Log("Checking player inventory for Iron Bars...");
        // Code to populate upgrades or tool repair costs goes here
    }
}