using System;
using UnityEngine;

namespace Gameplay
{
    public class GameplayTypingSystem : TypingSystem
    {
        [Header("Audio Manager Reference")]
        [SerializeField] AudioManager _audioManager;
        
        protected override void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
        {
            base.ExecuteCombo(comboSequence, comboIndex);
            
            // Implement the effect of the combo
            comboSequence.PlaySuccessAnimation();
            StartCoroutine(comboSequence.EndComboDelayed(0.1f));

            _audioManager?.PlaySoundExecuteCombo();
        }

        protected override void FailAllSequences()
        {
            base.FailAllSequences();
            
            // Play audio on context
            if (_audioManager != null)
            {
                if (_audioManager.Context == CurrentTypingContext)
                {
                    _audioManager.PlaySoundFailCombo();
                }
            }
            
            foreach (ComboSequence comboSequence in comboSequences)
            {
                if(comboSequence.Context == CurrentTypingContext)
                    comboSequence.EndComboInstantly();
            }
        }
    }
}