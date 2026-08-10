using UnityEngine;
using UnityEngine.UI;

public class TabManager : StationUI
{
    [SerializeField] private AnvilUI firstUI;
    [SerializeField] private GeodeUI secondUI;

    [SerializeField] private Button firstTabButton;
    [SerializeField] private Button secondTabButton;

    public enum Tab { First, Second }
    private Tab tab = Tab.First;

    public void FirstTab()
    {
        tab = Tab.First;
        UpdateTab();
    }

    public void SecondTab()
    {
        tab = Tab.Second;
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
        firstUI.gameObject.SetActive(false);
        secondUI.gameObject.SetActive(false);
    }

    private void UpdateTab()
    {
        switch (tab)
        {
            case Tab.First:
                firstUI.gameObject.SetActive(true);
                secondUI.gameObject.SetActive(false);
                firstUI.UpdateAnvilDisplay();
                break;
            case Tab.Second:
                firstUI.gameObject.SetActive(false);
                secondUI.gameObject.SetActive(true);
                secondUI.UpdateGeodeDisplay();
                break;
        }
    }
}
