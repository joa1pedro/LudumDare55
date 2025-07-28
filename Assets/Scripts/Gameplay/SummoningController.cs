using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummoningController : MonoBehaviour
{
    [SerializeField] List<Summon> Summons = new List<Summon>(); 
    [SerializeField] EnemyController enemyController;

    private void Awake()
    {
        foreach (var summon in Summons)
        {
            summon.Initialize();
        }
    }

    public void PerformSummon(int index)
    {
        Summons[index].SummonSelf();
        
        if (enemyController != null)
        {
            foreach (var enemy in enemyController.laneEnemies)
            {
                if (enemy.LaneIndex == index)
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
