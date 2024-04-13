using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] GameObject EnemyPrefab;
    [SerializeField] GameObject EnemyContainer;
    [SerializeField] Summoner Summoner;

    [SerializeField] float Speed = 2.0f;

    [SerializeField] float minSpawnTime = 3.0f;
    [SerializeField] float maxSpawnTime = 3.0f;

    [SerializeField] float dyingPosition = -3.0f;


    private List<GameObject> enemies = new List<GameObject>();  // List to track all enemies


    void Start()
    {
        StartCoroutine(SpawnEnemyRepeatedly());
    }

    void Update()
    {
        // Update each enemy in the list
        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (enemies[i] != null)
            {
                enemies[i].transform.Translate(new Vector3(-Speed, 0, 0) * Time.deltaTime);
                if (enemies[i].transform.position.x <= dyingPosition)
                {
                    Debug.Log("Enemy has crossed the map!");
                    Destroy(enemies[i]);
                    enemies.RemoveAt(i);  // Remove the enemy from the list after destroying it
                    if (Summoner != null)
                    {
                        Summoner.ReduceHP(5);
                    }
                }
            }
        }
    }

    IEnumerator SpawnEnemyRepeatedly()
    {
        while (true)  // Infinite loop to keep spawning enemies
        {
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));  // Wait for a random time between 3 and 6 seconds
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {

        GameObject newEnemy = Instantiate(EnemyPrefab, EnemyContainer.transform);
        newEnemy.SetActive(true);
        enemies.Add(newEnemy);  // Add the new enemy to the list
    }
}
