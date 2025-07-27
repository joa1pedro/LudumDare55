using System.Collections.Generic;
using GameData;

namespace Utils
{
    public static class SaveLoadUtils
    {
        public static void ResetSaveData()
        {
            PlayerData defaultData = new PlayerData
            {
                Summons = new List<SummonData>(),
            };

            IOManager.SavePlayerData(defaultData);
        }

        public static List<SummonData> GetPlayerData()
        {
            PlayerData data = IOManager.LoadPlayerData();
            return data.Summons;
        }

        public static void AddActiveSummonToSaveData(string summonName)
        {
            PlayerData data = IOManager.LoadPlayerData();

            SummonData existing = data.Summons.Find(s => s.name == summonName);

            if (existing == null)
            {
                data.Summons.Add(new SummonData(summonName, -1, selected: true, locked: false));
            }
            else
            {
                existing.selected = true;
            }

            IOManager.SavePlayerData(data);
        }

        public static void RemoveActiveSummonFromSaveData(string summonName)
        {
            PlayerData data = IOManager.LoadPlayerData();

            SummonData existing = data.Summons.Find(s => s.name == summonName);
            if (existing != null)
            {
                existing.selected = false;
                IOManager.SavePlayerData(data);
            }
        }
    }
}