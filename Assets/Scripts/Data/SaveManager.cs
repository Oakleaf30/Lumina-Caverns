using System.IO;
using UnityEngine;

public static class SaveManager
{
    static string Path => Application.persistentDataPath + "/save.json";
    static string TempPath => Application.persistentDataPath + "/temp.json";

    public static bool SaveExists() => File.Exists(Path);
    public static bool TempExists() => File.Exists(TempPath);

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Path, json);
    }

    public static SaveData Load()
    {
        if (!File.Exists(Path)) return new SaveData();
        string json = File.ReadAllText(Path);
        return JsonUtility.FromJson<SaveData>(json);
    }

    public static void TempSave(TempData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(TempPath, json);
    }

    public static TempData LoadTemp()
    {
        if (!File.Exists(TempPath)) return new TempData();
        string json = File.ReadAllText(TempPath);
        return JsonUtility.FromJson<TempData>(json);
    }

    public static void DeleteTempSave()
    {
        if (File.Exists(TempPath))
            File.Delete(TempPath);
    }
}