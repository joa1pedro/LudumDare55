using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] EnemyController enemyController;
    [SerializeField] Animator animator = default;

   private int laneIndex = -1;

    // Callback called by the Animation Clip
    public void DeSummonSelf()
    {
        gameObject.SetActive(false);
    }

    public void SummonSelf(int index)
    {
        gameObject.SetActive(true);
        laneIndex = index;
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
                if (enemy.LaneIndex == laneIndex)
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
