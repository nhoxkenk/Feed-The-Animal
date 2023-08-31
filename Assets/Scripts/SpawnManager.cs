using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemies;
    GameManager gameManager;
    //public GameObject powerUp;

    private float SpawnRange = 42.0f;
    private float ySpawn = 0.52f;

    int waveNumber = 1;
    int enemyCount;


    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("SpawnRandomEnemy", startDelay, enemySpawnTime);
        //InvokeRepeating("SpawnPowerUp", startDelay, powerUpSpawnTime);
        SpawnEnemyWave(waveNumber);
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameManager.gameOver)
        {
            enemyCount = FindObjectsOfType<EnemyBehaviour>().Length;
            if (enemyCount == 0)
            {
                SpawnEnemyWave(++waveNumber);
            }
        }
        
    }

    int randomIndex()
    {
        return Random.Range(0, enemies.Length);
    }

    void SpawnRandomEnemy(int index, float x, float z)
    {
        
        Vector3 spawnPos = new Vector3(x, ySpawn, z);

        Instantiate(enemies[index], spawnPos, enemies[index].gameObject.transform.rotation);
    }

    void SpawnEnemyWave(int enemyNumber)
    {
        float randomX = Random.Range(0, 2) == 0 ? SpawnRange : -SpawnRange;
        float randomZ = Random.Range(0, 2) == 0 ? SpawnRange : -SpawnRange;
        for (int i = 0; i < enemyNumber; i++)
        {
            int index = randomIndex();

            SpawnRandomEnemy(index, randomX, randomZ);
        }
    }

    //void SpawnPowerUp()
    //{
    //    float randomX = Random.Range(-xSpawnRange, xSpawnRange);
    //    float randomZ = Random.Range(-zPowerupRange, zPowerupRange);

    //    Vector3 spawnPos = new Vector3(randomX, ySpawn, randomZ);

    //    Instantiate(powerUp, spawnPos, powerUp.gameObject.transform.rotation);
    //}
}
