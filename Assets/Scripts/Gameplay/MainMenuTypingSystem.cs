using UnityEngine;

namespace Gameplay
{
    public class MainMenuTypingSystem : TypingSystem
    {
        [Header("Audio Manager Reference")]
        [SerializeField] AudioManager audioManager;
        
        protected override void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
        {
            // Implement the effect of the combo
            comboSequence.PlaySuccessAnimation();
            StartCoroutine(comboSequence.EndComboDelayed(0.1f));
            audioManager.PlaySoundExecuteCombo();
        }

        protected override void FailCombos()
        {
            audioManager.PlaySoundFailCombo();
            foreach (ComboSequence comboSequence in comboSequences)
            {
                comboSequence.EndComboInstantly();
            }
            canvasToShake.ShakeCanvas();
        }
    }
}