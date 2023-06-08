using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EventBus;

public class GameManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] PlayerStats playerStats;
    [SerializeField] LevelsData levelsData;
    public List<Transform> generators = new List<Transform>();

    //Variables to manage waves and levels
    public static int enemyCount = 0;
    bool canGoToNextWave;
    void Start()
    {
        //GameEventBus.Publish(GameEventType.PRESTART);
        GenerateEnemies();
        canGoToNextWave = false;
    }

    private void Update() {
        if (enemyCount <= 0 && canGoToNextWave)
        {
            StartCoroutine(NextWave());
            canGoToNextWave = false;
        }
    }

    private void OnEnable() {
        GameEventBus.Subscribe(GameEventType.STARTLEVEL, GenerateEnemies);
    }

    private void OnDisable() {
        GameEventBus.Unsubscribe(GameEventType.STARTLEVEL, GenerateEnemies);
    }

    public void StartLevel()
    {
        GameEventBus.Publish(GameEventType.STARTLEVEL);
    }

    public void GenerateEnemies()
    {
        //Generate the enemies based in the LevelData specifications
        Debug.Log("Level Started");

        //Get the data of the specific type of enemy in this level and wave
        for (int i  = 0; i < levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes.Count; i++)
        {
            //Put data information into local variables to make code more readable
            enemyType EnemyType = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].EnemyType;
            int quantity = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].quantity;
            float spawnTime = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].spawnTime;
            int generatorNum = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].generator;
            int tier = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].tier;
            float spawnInterval = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].spawnInterval;

            //Get generator from which to spawn
            Transform generatorTrans = generators[generatorNum];

            //Look for the enemy prefab to spawn
            GameObject enemyToSpawn;
            foreach (GameObject enemy in levelsData.enemyPrefabs)
            {
                if (enemy.GetComponent<BaseEnemy>().enemyData.myEnemyType == EnemyType)
                {
                    enemyToSpawn = enemy;

                    //Spawn enemies
                    StartCoroutine(spawnEnemies(spawnTime, spawnInterval, quantity, enemyToSpawn, generatorTrans));
                }
            }
        }
    }

    IEnumerator spawnEnemies(float time, float interval, int quantity, GameObject enemyToSpawn, Transform generatorTrans)
    {
        yield return new WaitForSeconds(time);

        for (int j = 1; j <= quantity; j++)
        {
            //Just created another local variable to set the local position
            GameObject enemySpawning = Instantiate(enemyToSpawn, generatorTrans);
            enemySpawning.transform.localPosition = new Vector3(0, 0, 0);
            canGoToNextWave = true;
            yield return new WaitForSeconds(interval);
        }

    }

    IEnumerator NextWave()
    {
        yield return new WaitForSeconds(levelsData.levels[playerStats.level].time);
        playerStats.wave++;
        GenerateEnemies();
    }
}
