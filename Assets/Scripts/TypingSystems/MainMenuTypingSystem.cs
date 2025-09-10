using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Gameplay
{
    public class MainMenuTypingSystem : TypingSystem
    {
        [Header("Audio Manager Reference")]
        [SerializeField] AudioManager _audioManager;
        [SerializeField] MainMenuUIController _mainMenuUIController;

        public bool EnabledTypingButtons;
        
        private Dictionary<string, (Action callback, int? context)> _navigationCallbacks;
        private List<Button> _buttons;
        protected override void Initialize()
        {
           
            _navigationCallbacks = new Dictionary<string, (Action, int?)>
            {
                // Navigation Callbacks are assigned by string inside the ComboSequence prefab
                // If you want to change the current typing context of the TypingSystem just assign the desired value with the callback 
                { "Return", (_mainMenuUIController.Return, 0) },
                { "ShowOptionsMenu", (_mainMenuUIController.ShowOptionsMenu, 1) },
                { "ShowSummonsSelect", (_mainMenuUIController.ShowInventory, 2) },
                { "LoadScene", (_mainMenuUIController.LoadScene, null) },
                { "DisableTyping", (DisableTypingButtons, null) },
                { "EnableTyping", (EnableTypingButtons, null) },
            };

            _buttons = new List<Button>();
            foreach (var comboSequence in _comboSequences)
            {
                var button = comboSequence.GetComponentInChildren<Button>();
                _buttons.Add(button);
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => InvokeNavigationCallback(button.GetComponentInParent<ComboSequence>().CallbackKey));
            }

            if (EnabledTypingButtons)
            {
                EnableTypingButtons();
            }
            else
            {
                DisableTypingButtons();
            }
        }

        private void InvokeNavigationCallback(string callbackKey)
        {
            if (_navigationCallbacks.TryGetValue(callbackKey, out var data))
            {
                data.callback?.Invoke();
                if (data.context.HasValue)
                    CurrentTypingContext = data.context.Value;
            }
        }

        public void DisableTypingButtons()
        {
            foreach (var btn in _buttons)
                btn.interactable = true;
            
            _mainMenuUIController.EnableTyping(false);
            base.SetActive(false);
        }
        
        public void EnableTypingButtons()
        {
            foreach (var btn in _buttons)
                btn.interactable = false;
            
            _mainMenuUIController.EnableTyping(true);
            base.SetActive(true);
        }
        
        protected override void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
        {
            if (_navigationCallbacks.TryGetValue(comboSequence.CallbackKey, out var data))
            {
                data.callback?.Invoke();
                if (data.context.HasValue)
                    CurrentTypingContext = data.context.Value;
            }
            
            comboSequence.PlaySuccessAnimation();
            StartCoroutine(comboSequence.EndComboDelayed(0.1f));
            _audioManager.PlaySoundExecuteCombo();
        }

        protected override void FailAllSequences()
        {
            base.FailAllSequences();
            
            // Play audio on context
            if (_audioManager.Context == CurrentTypingContext)
            {
                _audioManager.PlaySoundFailCombo();
            }
            
            foreach (ComboSequence comboSequence in _comboSequences)
            {
                if(comboSequence.Context == CurrentTypingContext)
                    comboSequence.EndComboInstantly();
            }
        }
    }
}