using System.Collections.Generic;
using System.IO;
using GameData;
using UnityEngine;

public static class IOManager
{
    private static string GameDataPath => Path.Combine(Application.persistentDataPath, "gamedata.json");
    private static string PlayerDataPath => Path.Combine(Application.persistentDataPath, "playerdata.json");

    public static PlayerData LoadGameData()
    {
        if (File.Exists(PlayerDataPath))
        {
            string json = File.ReadAllText(GameDataPath);
            return JsonUtility.FromJson<PlayerData>(json);
        }

        return new PlayerData();
    }
    
    public static void SavePlayerData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(PlayerDataPath, json);
    }

    public static PlayerData LoadPlayerData()
    {
        if (File.Exists(PlayerDataPath))
        {
            string json = File.ReadAllText(PlayerDataPath);
            return JsonUtility.FromJson<PlayerData>(json);
        }

        return new PlayerData();
    }
}