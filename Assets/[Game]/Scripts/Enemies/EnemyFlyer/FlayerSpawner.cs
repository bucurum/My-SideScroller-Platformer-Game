using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlayerSpawner : MonoBehaviour
{
    [SerializeField] private int numberOfEnemies;
    public GameObject enemyFlayer;
    public float spawnInterval;
    [SerializeField] Transform[] spawnPoints;
    private bool canSpawn = true;
    private float nextSpawnTime;
    private int spawnPoint;

    void Update()
    {
        SpawnWave();
    }

    private void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            Transform randomPoints = spawnPoints[spawnPoint];
            Instantiate(enemyFlayer, randomPoints.position, Quaternion.identity);
            numberOfEnemies--;
            spawnPoint++;
            nextSpawnTime = Time.time + spawnInterval;

            if (numberOfEnemies == 0)
            {
                canSpawn = false;
            }
        }
       
    }


}
