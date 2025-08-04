using System;
using System.Collections.Generic;
using GameData;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class SummonSelectorController : MonoBehaviour
{
    [SerializeField] private SummonButton summonButtonPrefab;
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private EventSystem eventSystem;
    private static readonly int MaxSelections = 3;
    private static Queue<SummonButton> _selectedButtons = new Queue<SummonButton>();

    private void Start()
    {
        List<SummonData> unlockedSummons = SaveLoadUtils.GetPlayerData();
        SaveLoadUtils.ResetSaveData();
        
        GOUtils.DestroyAllChildren(buttonsParent);
        
        foreach (var summon in unlockedSummons)
        {
            SummonButton newButton = Instantiate(summonButtonPrefab, buttonsParent.transform);
            newButton.Initialize(text: summon.name, selected: summon.selected, callback: SelectPower);
            if (summon.selected) _selectedButtons.Enqueue(newButton);
        }
    }

    public void SelectPower(SummonButton button)
    {
        if (!button.Selected)
        {
            if (_selectedButtons.Count >= MaxSelections)
            {
                var oldest = _selectedButtons.Dequeue();
                oldest.SetSelected(false);
            }

            _selectedButtons.Enqueue(button);
            button.SetSelected(true);
            SaveLoadUtils.AddActiveSummonToSaveData(button.SummonName);
        }
        else
        {
            // Rebuild the queue manually to remove this button
            Queue<SummonButton> newQueue = new();
            while (_selectedButtons.Count > 0)
            {
                SummonButton b = _selectedButtons.Dequeue();
                if (b != button)
                    newQueue.Enqueue(b);
            }

            _selectedButtons = newQueue;
            button.SetSelected(false);
            SaveLoadUtils.RemoveActiveSummonFromSaveData(button.SummonName);
        }
    }
}