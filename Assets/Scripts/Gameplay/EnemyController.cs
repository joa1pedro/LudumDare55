using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Transform enemySpanwer;
    
    [Header("Lane Y Position")]
    [SerializeField]
    private List<float> laneyPositions;

    [Header("Lane X Position")]
    [SerializeField]
    private float lanexPosition;

    [Header("Point System Reference")]
    [SerializeField]
    private PointSystem pointSystem;

    [Header("Summoner for reduging HP")]
    [SerializeField]
    private Summoner summoner;

    [Header("Audio Manager Reference")]
    [SerializeField]
    private AudioManager audioManager;

    private GameObject enemyPrefab;
    
    public List<LaneEnemy> laneEnemies = new List<LaneEnemy>();

    // Timer setup for periodic speed increase
    [SerializeField] private float speedIncreaseTimer = 0.0f;
    [SerializeField] private float timeToIncreaseSpeed = 5.0f;
    [SerializeField] private float speedIncrement = 20.0f;
    [SerializeField] private float maxSpeed = 1000.0f;
    [SerializeField] private float currentSpeed = 80.0f;

    private void Start()
    {
        for (int i = 0; i < laneyPositions.Count; i++)
        {
            StartCoroutine(SpawnEnemyRepeatedly(i));
        }
    }

    private void Update()
    {
        if (summoner.gameEnded) return;
        // Check if it's time to increase speed
        speedIncreaseTimer += Time.deltaTime;
        if (speedIncreaseTimer >= timeToIncreaseSpeed && currentSpeed < maxSpeed)
        {
            currentSpeed = Mathf.Min(currentSpeed + speedIncrement, maxSpeed);
            speedIncreaseTimer = 0;  // Reset the timer
        }
    }

    private IEnumerator SpawnEnemyRepeatedly(int laneIndex)
    {
        while (true)
        {
            if (summoner.gameEnded) break;
            yield return new WaitForSeconds(Random.Range(4.0f, 7.0f));
            SpawnEnemy(laneIndex);
        }
    }

    private void SpawnEnemy(int laneIndex)
    {
        if (summoner.gameEnded) return;
        float positionY = laneyPositions[laneIndex];
        Vector3 spawnPosition = new Vector3(lanexPosition, positionY, enemySpanwer.transform.position.z);
        GameObject newEnemyGameObject = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, enemySpanwer.transform);
        
        newEnemyGameObject.SetActive(true);
        LaneEnemy newLaneEnemy = newEnemyGameObject.GetComponent<LaneEnemy>();
        newLaneEnemy.Initialize(laneIndex, currentSpeed, this);
        laneEnemies.Add(newLaneEnemy);
    }

    //Method called to remove from the list in case enemy passes the lane without dying
    // Does not give points
    public void RemoveEnemy(LaneEnemy laneEnemy)
    {
        if (summoner.gameEnded) return;
        summoner.ReduceHP(10);
        laneEnemies.Remove(laneEnemy);
    }

    //Method called to remove from the list in case enemy has been killed
    public void KillEnemy(LaneEnemy laneEnemy)
    {
        if (summoner.gameEnded) return;
        laneEnemies.Remove(laneEnemy);
    }

    public void StartKillEnemy(LaneEnemy laneEnemy)
    {
        if (summoner.gameEnded) return;
        pointSystem.ReceivePoints(laneEnemy.PointsFromKilling);
        audioManager.PlaySoundEnemyDied();
    }

    public void SetEnemies(List<GameObject> enemyPrefabs)
    {
        foreach (var prefab in enemyPrefabs)
        {
            enemyPrefab = prefab;
        }
    }
}
