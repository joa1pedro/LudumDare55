using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    [Tooltip("List of enemy identifiers this level should spawn")]
    public string[] enemyIds;
}