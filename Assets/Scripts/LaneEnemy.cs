using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneEnemy : MonoBehaviour
{

    public int LaneIndex;  // Store the index of the lane
    [SerializeField] private float Speed = 200.0f;
    [SerializeField] private float dyingPosition = -300.0f;
    [SerializeField] public Animator Animator;

    private bool walk = true;
    private EnemyController MyEnemyController;

    public void Initialize(int laneIndex, EnemyController myController)
    {
        this.LaneIndex = laneIndex;
        this.Animator = this.GetComponent<Animator>();
        MyEnemyController = myController;
    } 

    void Update()
    {
        if (walk)
        {
            this.transform.Translate(new Vector3(-Speed, 0, 0) * Time.deltaTime);
            if (transform.position.x <= dyingPosition)
            {
                // Self-destruct when crossing the threshold
                Killed();
            }
        }
    }

    public void StopWalk()
    {
        walk = false;
    }

    public void Killed()
    {
        MyEnemyController.RemoveEnemy(this);
        Destroy(gameObject);
    }

}
