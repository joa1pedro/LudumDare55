using UnityEngine;

public class GameplayComboSequence : ComboSequence
{
    [SerializeField] Star starsEffect;
    
    public override void PlaySuccessAnimation()
    {
        base.PlaySuccessAnimation();
        starsEffect.PlayStarAnimation();
    }
}
