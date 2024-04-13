using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float speed = 2.0f;
    private List<GameObject> enemies = new List<GameObject>();  // List to track all enemies
    public Summoner summoner;

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
                enemies[i].transform.Translate(new Vector3(-speed, 0, 0) * Time.deltaTime);
                if (enemies[i].transform.position.x <= -3.0f)
                {
                    Debug.Log("Enemy has crossed the map!");
                    Destroy(enemies[i]);
                    enemies.RemoveAt(i);  // Remove the enemy from the list after destroying it
                    if (summoner != null)
                    {
                        summoner.ReduceHP(5);
                    }
                }
            }
        }
    }

    IEnumerator SpawnEnemyRepeatedly()
    {
        while (true)  // Infinite loop to keep spawning enemies
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f));  // Wait for a random time between 3 and 6 seconds
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = new Vector3(8.57f, -3.06f, -1);
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.SetActive(true);
        enemies.Add(newEnemy);  // Add the new enemy to the list
    }
}
