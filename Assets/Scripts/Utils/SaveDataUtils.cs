using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Utils
{
    public static class SaveDataUtils
    {
        public static List<string> LoadUnlockedSummons()
        {
            // TODO Grab this from somewhere
            return new List<string>{"Paralelepipedo", "Otorrinolaringologista", "Combo", "Globers", "Misty"};
        }
        
        public static void AddSummonSave(string powerId)
        {
            PlayerData data = SaveManager.Load();

            if (!data.selectedPowers.Contains(powerId))
            {
                data.selectedPowers.Add(powerId);
                SaveManager.Save(data);
            }
        }

        public static void RemoveSummonSave(string powerId)
        {
            PlayerData data = SaveManager.Load();

            if (data.selectedPowers.Contains(powerId))
            {
                data.selectedPowers.Remove(powerId);
                SaveManager.Save(data);
            }
        }
    }
}