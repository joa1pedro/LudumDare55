using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
    public class MainMenuTypingSystem : TypingSystem
    {
        [Header("Audio Manager Reference")]
        [SerializeField] AudioManager audioManager;
        [SerializeField] MainMenuController mainMenuController;
        private Dictionary<string, (Action callback, int? context)> navigationCallbacks;
        
        private List<Button> buttons;
        protected override void Initialize()
        {
            navigationCallbacks = new Dictionary<string, (Action, int?)>
            {
                // Navigation Callbacks are assigned by string inside the ComboSequence prefab
                // If you want to change the current typing context of the TypingSystem just assign the desired value with the callback 
                { "Return", (mainMenuController.Return, 0) },
                { "ShowOptionsMenu", (mainMenuController.ShowOptionsMenu, 1) },
                { "ShowSummonsSelect", (mainMenuController.ShowSummonsSelect, 2) },
                { "LoadScene", (mainMenuController.LoadScene, null) },
                { "DisableTyping", (DisableTypingButtons, null) },
                { "EnableTyping", (EnableTypingButtons, null) },
            };

            buttons = new List<Button>();
            foreach (var comboSequence in comboSequences)
            {
                var button = comboSequence.GetComponentInChildren<Button>();
                buttons.Add(button);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => InvokeNavigationCallback(button.GetComponentInParent<ComboSequence>().CallbackKey));
            }
            
            EnableTypingButtons();
        }

        private void InvokeNavigationCallback(string callbackKey)
        {
            if (navigationCallbacks.TryGetValue(callbackKey, out var data))
            {
                data.callback?.Invoke();
                if (data.context.HasValue)
                    CurrentTypingContext = data.context.Value;
            }
        }

        public void DisableTypingButtons()
        {
            foreach (var btn in buttons)
                btn.interactable = true;
            
            mainMenuController.EnableTyping(false);
            ActivateSystem(false);
        }
        
        public void EnableTypingButtons()
        {
            foreach (var btn in buttons)
                btn.interactable = false;
            
            mainMenuController.EnableTyping(true);
            ActivateSystem(true);
        }
        
        protected override void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
        {
            if (navigationCallbacks.TryGetValue(comboSequence.CallbackKey, out var data))
            {
                data.callback?.Invoke();
                if (data.context.HasValue)
                    CurrentTypingContext = data.context.Value;
            }
            
            comboSequence.PlaySuccessAnimation();
            StartCoroutine(comboSequence.EndComboDelayed(0.1f));
            audioManager.PlaySoundExecuteCombo();
        }

        protected override void FailCombos()
        {
            base.FailCombos();
            
            // Play audio on context
            if (audioManager.Context == CurrentTypingContext)
            {
                audioManager.PlaySoundFailCombo();
            }
            
            foreach (ComboSequence comboSequence in comboSequences)
            {
                if(comboSequence.Context == CurrentTypingContext)
                    comboSequence.EndComboInstantly();
            }
        }
    }
}