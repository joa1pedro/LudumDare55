using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] GameObject EnemyContainer;
    [SerializeField] List<float> laneyPositions;
    [SerializeField] float lanexPosition;
    
    public List<LaneEnemy> LaneEnemies = new List<LaneEnemy>(); 

    void Start()
    {
        for (int i = 0; i < laneyPositions.Count; i++)
        {
            StartCoroutine(SpawnEnemyRepeatedly(i));
        }
    }

    IEnumerator SpawnEnemyRepeatedly(int laneIndex)
    {
        while (true)  
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 6.0f));
            SpawnEnemy(laneIndex);
        }
    }

    void SpawnEnemy(int laneIndex)
    {
        float positionY = laneyPositions[laneIndex];
        Vector3 spawnPosition = new Vector3(lanexPosition, positionY, EnemyContainer.transform.position.z);
        GameObject newEnemyGameObject = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity, EnemyContainer.transform);

        newEnemyGameObject.SetActive(true);
        LaneEnemy newLaneEnemy = newEnemyGameObject.GetComponent<LaneEnemy>();
        newLaneEnemy.Initialize(laneIndex, this);
        LaneEnemies.Add(newLaneEnemy);
    }

    public void RemoveEnemy(LaneEnemy laneEnemy)
    {
        LaneEnemies.Remove(laneEnemy);
    }
}
