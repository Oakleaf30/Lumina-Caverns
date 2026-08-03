using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    public static GameSession Instance { get; private set; }

    public RunState runState;
    public SaveData saveData;

    [SerializeField] private EquipmentRegistry pickaxeRegistry;

    [SerializeField] private float autosaveInterval = 1f;
    private float _autosaveTimer;

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
        runState.Initialise();

        if (SaveManager.SaveExists())
        {
            var loaded = SaveManager.Load();
            SaveConverter.ApplyToRunState(loaded, runState);

            if (SaveManager.TempExists())
            {
                var tempLoaded = SaveManager.LoadTemp();
                SaveConverter.ApplyTemp(tempLoaded, runState);
                ScreenFader.Instance.TransitionToScene("Mine");
            }
        }
        else
        {
            SeedNewGameDefaults();
        }
    }

    void SeedNewGameDefaults()
    {
        runState.pickaxe = pickaxeRegistry.pickaxes[0];
        runState.tier = runState.pickaxe.tiers[0];
        runState.pickaxeDurability = runState.tier.maxDurability;
    }

    public void SaveGame()
    {
        var data = SaveConverter.ToSaveData(runState);
        SaveManager.Save(data);
    }

    public void TempSave()
    {
        var data = SaveConverter.ToTempSave(runState);
        SaveManager.TempSave(data);
    }

    void Update()
    {
        _autosaveTimer += Time.deltaTime;
        if (_autosaveTimer >= autosaveInterval)
        {
            _autosaveTimer = 0f;

            if (SceneManager.GetActiveScene().name == "Mine")
            TempSave();
        }
    }

    public void ExitMines()
    {
        SaveGame();
        SaveManager.DeleteTempSave();
        ScreenFader.Instance.TransitionToScene("Base");
        runState.currentFloor = 0;
    }
}