using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "playerdata.json");
    
    public static void ResetSaveData()
    {
        PlayerData defaultData = new PlayerData
        {
            selectedPowers = new List<string>(),
        };

        Save(defaultData);
    }
    
    public static void Save(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
    }

    public static PlayerData Load()
    {
        if (File.Exists(SavePath))
        {
            string json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<PlayerData>(json);
        }

        return new PlayerData();
    }
}