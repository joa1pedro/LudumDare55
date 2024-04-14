using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    private EnemyController enemyController;
    void Start()
    {
        enemyController = FindObjectOfType<EnemyController>(); // Ensure there is only one EnemyMover in the scene
        if (enemyController == null)
        {
            Debug.LogError("EnemyMover not found in the scene!");
        }
    }

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
        Debug.Log("Golem Attack!");
        animator.Play("GolemAttack");
        Debug.Log(this.LaneIndex);
        if (enemyController != null)
        {
            foreach (var enemy in enemyController.enemies)
            {
                Debug.Log(enemy.LaneIndex);
                if (enemy.LaneIndex == this.LaneIndex)
                {
                    enemy.Animator.Play("SkeletonDie");
                    Debug.Log($"Destruiu Inimigo na Lane {this.LaneIndex}");
                    enemy.EnemyObject.GetComponent<Enemy>().StopWalk();
                    enemyController.enemies.Remove(enemy);
                    break;
                }
            }

        }
    }
}
