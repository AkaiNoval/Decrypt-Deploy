using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> enemies = new List<GameObject>();
    [SerializeField] GameObject spawnPos;

    [SerializeField] int spawnAmount;
    int currentAmount;
    [SerializeField] float minSpawnInterval;
    [SerializeField] float maxSpawnInterval;

    [SerializeField] float minSpawnYRange;
    [SerializeField] float maxSpawnYRange;
    [SerializeField] bool shouldDrawGizmos;

    bool shouldSpawnEnemy;
    bool isCoroutineRunning;

    private void OnEnable()
    {
        GameManager.GameStarted += ShouldSpawnEnemy;
    }
    private void OnDisable()
    {
        GameManager.GameStarted -= ShouldSpawnEnemy;
    }

    private void Update()
    {
        if (!shouldSpawnEnemy) return;
        if (isCoroutineRunning) return;
        StartSpawnEnemy();
    }
    void ShouldSpawnEnemy()
    {
        shouldSpawnEnemy = true;
    }
    void StartSpawnEnemy()
    {

        Debug.Log("Started Spawn Enemy");
        StartCoroutine(SpawnEnemy());
    }
    private void OnDrawGizmos()
    {
        if (!shouldDrawGizmos) return;
        Vector3 pos1 = new Vector3(spawnPos.transform.position.x,  minSpawnYRange, spawnPos.transform.position.z);
        Vector3 pos2 = new Vector3(spawnPos.transform.position.x,  maxSpawnYRange, spawnPos.transform.position.z);
        Gizmos.DrawLine(pos1, pos2);
    }
    IEnumerator SpawnEnemy()
    {
        while (currentAmount < spawnAmount)
        {
            isCoroutineRunning=true;
            Vector3 randomPos = new Vector3(spawnPos.transform.position.x, RandomeSpawnValue(minSpawnYRange, maxSpawnYRange), spawnPos.transform.position.z);
            yield return new WaitForSeconds(RandomeSpawnValue(minSpawnInterval, maxSpawnInterval));
            int enemyIndex = Random.Range(0, enemies.Count);
            Instantiate(enemies[enemyIndex], randomPos, Quaternion.identity);
            currentAmount++;
            isCoroutineRunning = false;
        }

    }
    float RandomeSpawnValue(float minValue, float maxValue) => Random.Range(minValue, maxValue);

 

}
