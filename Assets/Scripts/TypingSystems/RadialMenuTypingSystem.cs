using System;

namespace Gameplay
{
    public class RadialMenuTypingSystem : TypingSystem
    {
        protected override void FailAllSequences()
        {
            base.FailAllSequences();
            
            foreach (ComboSequence comboSequence in _comboSequences)
            {
                if(comboSequence.Context == CurrentTypingContext)
                    comboSequence.EndComboInstantly();
            }
        }
        
        protected override void ExecuteCombo(ComboSequence comboSequence, int comboIndex)
        {
            comboSequence.PlaySuccessAnimation();
            StartCoroutine(comboSequence.EndComboDelayed(0.1f));
        }
        
    }
}