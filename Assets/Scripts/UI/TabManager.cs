using UnityEngine;
using UnityEngine.UI;

public class TabManager : StationUI
{
    [SerializeField] private AnvilUI repairUI;
    [SerializeField] private GeodeUI geodeUI;

    [SerializeField] private Button repairTabButton;
    [SerializeField] private Button geodeTabButton;

    public enum Tab { Repair, Geode }
    private Tab tab = Tab.Repair;

    public void RepairTab()
    {
        tab = Tab.Repair;
        UpdateTab();
    }

    public void GeodeTab()
    {
        tab = Tab.Geode;
        UpdateTab();
    }

    protected override void OpenMenu()
    {
        base.OpenMenu();
        UpdateTab();
    }

    public override void CloseMenu()
    {
        base.CloseMenu();
        repairUI.gameObject.SetActive(false);
        geodeUI.gameObject.SetActive(false);
    }

    private void UpdateTab()
    {
        switch (tab)
        {
            case Tab.Repair:
                repairUI.gameObject.SetActive(true);
                geodeUI.gameObject.SetActive(false);
                repairUI.UpdateAnvilDisplay();
                break;
            case Tab.Geode:
                repairUI.gameObject.SetActive(false);
                geodeUI.gameObject.SetActive(true);
                geodeUI.UpdateGeodeDisplay();
                break;
        }
    }
}
