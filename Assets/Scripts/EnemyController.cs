using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] GameObject EnemyContainer;

    [Header("Lane Y Position")]
    [SerializeField] List<float> laneyPositions;

    [Header("Lane X Position")]
    [SerializeField] float lanexPosition;

    [Header("Point System Reference")]
    [SerializeField] PointSystem pointSystem;

    [Header("Summoner for reduging HP")]
    [SerializeField] Summoner summoner;

    [Header("Audio Manager Reference")]
    [SerializeField] AudioManager audioManager;

    public List<LaneEnemy> LaneEnemies = new List<LaneEnemy>();

    // Timer setup for periodic speed increase
    [SerializeField] float speedIncreaseTimer = 0.0f;
    [SerializeField] float timeToIncreaseSpeed = 5.0f;
    [SerializeField] float speedIncrement = 20.0f;
    [SerializeField] float maxSpeed = 1000.0f;
    [SerializeField] float currentSpeed = 80.0f;

    void Start()
    {
        for (int i = 0; i < laneyPositions.Count; i++)
        {
            StartCoroutine(SpawnEnemyRepeatedly(i));
        }
    }

    void Update()
    {
        // Check if it's time to increase speed
        speedIncreaseTimer += Time.deltaTime;
        if (speedIncreaseTimer >= timeToIncreaseSpeed && currentSpeed < maxSpeed)
        {
            currentSpeed = Mathf.Min(currentSpeed + speedIncrement, maxSpeed);
            speedIncreaseTimer = 0;  // Reset the timer
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
        newLaneEnemy.Initialize(laneIndex, currentSpeed, this);
        LaneEnemies.Add(newLaneEnemy);
    }

    //Method called to remove from the list in case enemy passes the lane without dying
    // Does not give points
    public void RemoveEnemy(LaneEnemy laneEnemy)
    {
        summoner.ReduceHP(10);
        LaneEnemies.Remove(laneEnemy);
    }

    //Method called to remove from the list in case enemy has been killed
    public void KillEnemy(LaneEnemy laneEnemy)
    {
        LaneEnemies.Remove(laneEnemy);
    }

    public void StartKillEnemy(LaneEnemy laneEnemy)
    {
        pointSystem.ReceivePoints(laneEnemy.PointsFromKilling);
        audioManager.PlaySoundEnemyDied();
    }
}
