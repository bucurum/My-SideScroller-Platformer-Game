using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlayerSpawner : MonoBehaviour
{
    private int numberOfEnemies;
    [SerializeField] int totalEnemy;
    public GameObject enemyFlayer;
    public float spawnInterval;
    [SerializeField] Transform[] spawnPoints;
    private bool canSpawn = true;
    private float nextSpawnTime;
    private int spawnPoint = 0;
    private bool isPlayerNear = false;

    void Start()
    {
        numberOfEnemies = totalEnemy;
    }
    void Update()
    {
        StartCoroutine(Spawn());
        GameObject[] totalEnemy = GameObject.FindGameObjectsWithTag("EnemyFlyer");
        if (totalEnemy.Length == 0 && !canSpawn)
        {
            canSpawn = true;
            
        }
    }

    IEnumerator Spawn()
    {
        SpawnWave();
        yield return new WaitForSeconds(spawnInterval);
    }
    private void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time && isPlayerNear)
        {   
            Instantiate(enemyFlayer, spawnPoints[spawnPoint].position, Quaternion.identity);
            numberOfEnemies--;
            spawnPoint++;
            nextSpawnTime = Time.time + spawnInterval;

            if (numberOfEnemies == 0)
            {
                canSpawn = false;
                numberOfEnemies = totalEnemy;
                spawnPoint = 0;
            }
            
        }  
    }
    
   

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true; 
        }
    }
    void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = false;

        }
    }

}
