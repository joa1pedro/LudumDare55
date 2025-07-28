using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Collections.Generic;
using UnityEngine;

enum AnimationType
{
    Attack,
    Idle,
    Die
}

public class Summon : MonoBehaviour
{
    [SerializeField] Animator animator = default;
    private Dictionary<AnimationType, string> animationNames;
   
    public void Initialize()
    {
        animationNames = new Dictionary<AnimationType, string>();
        var clips = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationType type in Enum.GetValues(typeof(AnimationType)))
        {
            bool found = false;

            foreach (var clip in clips)
            {
                if (clip.name.Contains(type.ToString()))
                {
                    animationNames[type] = clip.name;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                Debug.LogWarning($"{type} animation missing for Summon Prefab, please name it <summonName>AnimationType");
            }
        }
    }



   // Callback called by the Animation Clip
    public void DeSummonSelf()
    {
        gameObject.SetActive(false);
    }

    public void SummonSelf()
    {
        gameObject.SetActive(true);
        Attack();
    }

    public void Attack()
    {
        animator.Play(animationNames[AnimationType.Attack]);
    }
    
    public void Idle()
    {
        animator.Play(animationNames[AnimationType.Idle]);
    }
    
    public void Die()
    {
        animator.Play(animationNames[AnimationType.Die]);
    }
}
