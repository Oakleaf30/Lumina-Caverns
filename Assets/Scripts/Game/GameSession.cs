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
        // Temp
        runState.pickaxe = basePickaxe;
        runState.tier = basePickaxe.tiers[0];
    }
}