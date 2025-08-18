using System.Collections.Generic;
using UnityEngine;

public class EnemyDatabase : ScriptableObject
{
    [System.Serializable]
    public struct EnemyEntry
    {
        public string id;
        public GameObject prefab;
    }

    [Tooltip("List of enemies with unique IDs and prefabs")]
    public EnemyEntry[] enemies;

    private Dictionary<string, GameObject> _lookup;

    public void Initialize()
    {
        _lookup = new Dictionary<string, GameObject>();
        foreach (var entry in enemies)
            _lookup[entry.id] = entry.prefab;
    }

    public GameObject GetEnemyPrefab(string id)
    {
        if (_lookup == null)
            Initialize();

        if (_lookup.TryGetValue(id, out var prefab))
            return prefab;

        Debug.LogWarning($"Enemy ID '{id}' not found in database.");
        return null;
    }
}