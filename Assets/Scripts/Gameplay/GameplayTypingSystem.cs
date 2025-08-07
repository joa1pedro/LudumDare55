using UnityEngine;

namespace Gameplay
{
    public class GameplayTypingSystem : TypingSystem
    {
        [Header("Summons Controller Reference")]
        [SerializeField] SummoningController summoningController;

        [Header("Audio Manager Reference")]
        [SerializeField] AudioManager audioManager;
        
        protected override void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
        {
            // Implement the effect of the combo
            comboSequence.PlaySuccessAnimation();
            StartCoroutine(comboSequence.EndComboDelayed(0.1f));

            summoningController?.PerformSummon(comboIndex);
            audioManager?.PlaySoundExecuteCombo();
        }

        protected override void FailAllSequences()
        {
            base.FailAllSequences();
            
            // Play audio on context
            if (audioManager != null)
            {
                if (audioManager.Context == CurrentTypingContext)
                {
                    audioManager.PlaySoundFailCombo();
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