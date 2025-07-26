using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
    [SerializeField] Animator animator = default;

    bool IsEnabled = false;
    private int LaneIndex = -1;

    // Callback called by the Animation Clip
    public void DeSummonSelf()
    {
        IsEnabled = false;
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
            foreach (var enemy in enemyController.laneEnemies)
            {
                if (enemy.LaneIndex == this.LaneIndex)
                {
                    //Play Animation
                    enemy.Animator.Play("SkeletonDie");
                    enemy.StopWalk();
                    break;
                }
            }

        }
    }
}
