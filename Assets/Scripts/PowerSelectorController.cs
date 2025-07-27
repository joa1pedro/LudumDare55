using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils;

public class PowerSelectorController : MonoBehaviour
{
    [SerializeField] private SummonButton summonButtonPrefab;
    [SerializeField] private GameObject buttonsParent;
    [SerializeField] private EventSystem eventSystem;
    private static readonly int maxSelections = 3;
    private static Queue<SummonButton> selectedButtons = new Queue<SummonButton>();

    private void Start()
    {
        SaveManager.ResetSaveData();
        List<String> unlockedSummons = SaveDataUtils.LoadUnlockedSummons();

        bool first = true;
        GOUtils.DestroyAllChildren(buttonsParent);
        foreach (var summon in unlockedSummons)
        {
            SummonButton newButton = Instantiate(summonButtonPrefab, buttonsParent.transform);
            newButton.Initialize(text: summon, selected: false, callback: SelectPower);
            if (first)
            {
                eventSystem.firstSelectedGameObject = newButton.gameObject;
                first = false;
            }
        }
    }

    public void SelectPower(SummonButton button)
    {
        if (!button.Selected)
        {
            if (selectedButtons.Count >= maxSelections)
            {
                var oldest = selectedButtons.Dequeue();
                oldest.SetSelected(false);
            }

            selectedButtons.Enqueue(button);
            button.SetSelected(true);
            SaveDataUtils.AddSummonSave(button.SummonName);
        }
        else
        {
            // Rebuild the queue manually to remove this button
            Queue<SummonButton> newQueue = new();
            while (selectedButtons.Count > 0)
            {
                SummonButton b = selectedButtons.Dequeue();
                if (b != button)
                    newQueue.Enqueue(b);
            }

            selectedButtons = newQueue;
            button.SetSelected(false);
            SaveDataUtils.RemoveSummonSave(button.SummonName);
        }
    }
}