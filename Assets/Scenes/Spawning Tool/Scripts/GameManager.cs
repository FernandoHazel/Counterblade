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
    [SerializeField] TextMeshProUGUI debuggingText;
    public List<Transform> generators = new List<Transform>();

    //Variables to manage waves and levels
    public static int enemyCount = 0;
    void Start()
    {
        //Generate the first wave of the level
        debuggingText.text = $"Wave: {playerStats.wave + 1}";
        GenerateEnemies();
    }


    public void GenerateEnemies()
    {
        //Generate the enemies based in the LevelData specifications
        Debug.Log("Generating enemies");

        //Get the data of the specific type of enemy in this level and wave
        for (int i  = 0; i < levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes.Count; i++)
        {
            //Pasamos la información que de cada oleada a variables más legibles
            enemyType EnemyType = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].EnemyType;
            int quantity = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].quantity;
            float spawnTime = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].spawnTime;
            int generatorNum = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].generator;
            int tier = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].tier;
            float spawnInterval = levelsData.levels[playerStats.level].waves[playerStats.wave].EnemySpawnTypes[i].spawnInterval;

            //Tomamos el generador del cual debe de spawnear cada enemigo
            Transform generatorTrans = generators[generatorNum];

            //Tomamos el prefab del enemigo a spawnear
            GameObject enemyToSpawn;

            foreach (GameObject enemy in levelsData.enemyPrefabs)
            {
                //Recorremos el array con los prefabs y si encontramos al que hay que espawnear en dicha oleada
                //lo tomamos
                if (enemy.GetComponent<Enemy>().myStats.myEnemyType == EnemyType)
                {
                    enemyToSpawn = enemy;

                    //Spawn enemies
                    StartCoroutine(spawnEnemies(spawnTime, spawnInterval, quantity, enemyToSpawn, generatorTrans));
                }
            }
        }
    }

    //En esta corrutina se gestiona la velocidad de spawn de cada enemigo
    //Basado en el tiempo entre oleadas e intervalo
    IEnumerator spawnEnemies(float time, float interval, int quantity, GameObject enemyToSpawn, Transform generatorTrans)
    {
        //Primero esperamos el tiempo entre oleadas
        yield return new WaitForSeconds(time);

        for (int j = 1; j <= quantity; j++)
        {
            //Just created another local variable to set the local position
            GameObject enemySpawning = Instantiate(enemyToSpawn, generatorTrans);
            enemySpawning.transform.localPosition = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(interval);
        }

    }

    private void NextWave()
    {
        if (enemyCount <= 0)
        {
            playerStats.wave++;
            debuggingText.text = $"Wave: {playerStats.wave}";
            GenerateEnemies();
        }
    }
}
