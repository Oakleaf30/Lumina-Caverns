using UnityEditor.Overlays;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public RunState runState;
    public SaveData saveData;

    public PickaxeData basePickaxe;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        runState = new RunState();
        runState.ResetForNewRun();

        if (SaveManager.SaveExists())
        {
            var loaded = SaveManager.Load();
            SaveConverter.ApplyToRunState(loaded, runState); // overwrites persistent fieldsa
        }
        else
        {
            SeedNewGameDefaults();
        }
    }

    void SeedNewGameDefaults()
    {
        // Temp
        runState.pickaxe = basePickaxe;
        runState.tier = basePickaxe.tiers[0];
    }

    public void SaveGame()
    {
        var data = SaveConverter.ToSaveData(runState);
        SaveManager.Save(data);
    }


}