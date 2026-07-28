using System.IO;
using UnityEngine;

public static class SaveManager
{
    static string Path => Application.persistentDataPath + "/save.json";

    public static bool SaveExists() => File.Exists(Path);

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
}