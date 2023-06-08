using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "LevelsData/New LevelsData", order = 1)]
public class LevelsData : ScriptableObject

{
    public GameObject[] enemyPrefabs;
    public List<Level> levels = new List<Level>();
}

[System.Serializable]
public class Level
{
    [Tooltip("Rest time between waves (sec)")]
    public float time;
    public List<Wave> waves = new List<Wave>();
}

[System.Serializable]
public class Wave
{
    public List<EnemySpawn> EnemySpawnTypes = new List<EnemySpawn>();
}

[System.Serializable]
public class EnemySpawn
{
    public enemyType EnemyType;
    [Tooltip("quantity of this enemy in this wave")]
    public int quantity;
    [Tooltip("seconds after the beginning of the wave to spawn this enemy")]
    public float spawnTime;
    [Tooltip("From which generator this enemy will spawn")]
    public int generator = 1;
    [Tooltip("This is the level of this group of enemies")]
    public int tier = 0;
    [Tooltip("Spawn time between each enemy")]
    public float spawnInterval = 0.5f;
}