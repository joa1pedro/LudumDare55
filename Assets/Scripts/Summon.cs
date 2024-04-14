using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
    [SerializeField] Animator animator = default;

    bool IsEnabled = false;
    private int LaneIndex = -1;

    public void DeSummonSelf()
    {
        IsEnabled = false;
        // PlayDeSummonAnimation()
        this.gameObject.SetActive(false);
    }

    public void SummonSelf(int index)
    {
        IsEnabled = true;
        this.gameObject.SetActive(true);
        this.LaneIndex = index;
        PlaySummonAnimation();
    }

    public void PlaySummonAnimation()
    {
        Attack();
    }

    public void Attack()
    {
        animator.Play("GolemAttack");
        if (enemyController != null)
        {
            foreach (var enemy in enemyController.LaneEnemies)
            {
                if (enemy.LaneIndex == this.LaneIndex)
                {
                    //Play Animation
                    enemy.Animator.Play("SkeletonDie");
                    enemy.StopWalk();

                    //Remove from the Controller
                    enemyController.RemoveEnemy(enemy);
                    break;
                }
            }

        }
    }
}
