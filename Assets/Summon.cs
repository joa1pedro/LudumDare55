using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] Animator animator = default;
    bool IsEnabled = false;

    public void DeSummonSelf()
    {
        IsEnabled = false;
        // PlayDeSummonAnimation()
        this.gameObject.SetActive(false);
    }

    public void SummonSelf()
    {
        IsEnabled = true;
        this.gameObject.SetActive(true);
        PlaySummonAnimation();

        // Invoke DeSummonSelf after 2 seconds
        Invoke("DeSummonSelf", 1f);
    }

    public void PlaySummonAnimation()
    {
        animator.Play("GolemAttack");
    }


    // Callback function that will be called when the Animation Event occurs
    public void OnAnimationEnd()
    {
        // Do something when the animation ends
        Debug.Log("Animation ended!");
    }

    public void PlayDeSummonAnimation()
    {

    }
}
