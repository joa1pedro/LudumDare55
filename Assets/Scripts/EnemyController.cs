using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneEnemy
{
    public GameObject EnemyObject;
    public int LaneIndex;  // Store the index of the lane
    public Animator Animator;

    public LaneEnemy(GameObject enemyObject, int laneIndex)
    {
        EnemyObject = enemyObject;
        LaneIndex = laneIndex;
        Animator = enemyObject.GetComponent<Animator>();
    }
}

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] GameObject EnemyContainer;
    [SerializeField] List<float> laneyPositions;
    [SerializeField] float lanexPosition;
    public List<LaneEnemy> enemies = new List<LaneEnemy>();  // List to track all enemies along with their lanes

    void Start()
    {
        for (int i = 0; i < laneyPositions.Count; i++)
        {
            StartCoroutine(SpawnEnemyRepeatedly(i));
        }
    }

    IEnumerator SpawnEnemyRepeatedly(int laneIndex)
    {
        while (true)  // Infinite loop to keep spawning enemies
        {
            yield return new WaitForSeconds(Random.Range(3.0f, 6.0f));  // Wait for a random time
            SpawnEnemy(laneIndex);
        }
    }

    void SpawnEnemy(int laneIndex)
    {
        float positionY = laneyPositions[laneIndex];  // Retrieve the position using the index
        Vector3 spawnPosition = new Vector3(lanexPosition, positionY, EnemyContainer.transform.position.z);
        GameObject newEnemy = Instantiate(EnemyPrefab, spawnPosition, Quaternion.identity, EnemyContainer.transform);
        newEnemy.SetActive(true);
        LaneEnemy laneEnemy = new LaneEnemy(newEnemy, laneIndex);  // Pass the index to the constructor
        enemies.Add(laneEnemy);
    }
}
