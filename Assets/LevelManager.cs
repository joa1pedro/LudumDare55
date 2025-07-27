using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private EnemyDatabase enemyDatabase;
    [SerializeField] private LevelData currentLevelData;
    [SerializeField] private EnemyController enemyController;
    
    
    private PlayerData currentPlayerData;
    
    
    private List<GameObject> enemyPrefabs;
    public void Initialize(LevelData levelData)
    {
        if (levelData == null || levelData.enemyIds == null)
        {
            Debug.LogError("Invalid level data.");
            return;
        }

        enemyPrefabs = new List<GameObject>();
        foreach (var enemyId in levelData.enemyIds)
        {
            enemyPrefabs.Add(enemyDatabase.GetEnemyPrefab(enemyId));
        }
        
        enemyController.SetEnemies(enemyPrefabs);
    }
    
    void Start()
    {
        currentPlayerData = IOManager.LoadPlayerData();
        Initialize(currentLevelData);
        
    }
}