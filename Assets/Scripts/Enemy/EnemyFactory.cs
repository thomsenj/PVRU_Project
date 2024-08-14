using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class EnemyFactory : NetworkBehaviour
{
    public List<GameObject> prefabs;

    public GameObject spawnTarget;

    public int enemyCount = 0;
    public int enemyMax = 5;
    public float spawnRadius = 10f;
    public float spawnInterval = 8f; 

    private ScoreManager scoreManager;
    private List<GameObject> enemyGameObjects;

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ScoreManager>();
        enemyGameObjects = new List<GameObject>();
        StartCoroutine(SpawnEnemies());
    }

    private void Update() 
    {
        enemyMax = scoreManager.GetEnemyCount();
    }

    public void EnemyDied(GameObject enemy) 
    {
        enemyGameObjects.Add(enemy);
        if(enemyCount > 0) 
        {
            enemyCount--;
        }
    }

    public void ResetFactory(GameObject spawner) 
    {
        spawnTarget = spawner;
        enemyCount = 0; 
    }

    public void UpdateEnemies(List<GameObject> enemies) 
    {
        foreach (GameObject gameObject in enemies) 
        {
            gameObject.SetActive(false);
        }
        enemyGameObjects.AddRange(enemies);
    }

    IEnumerator SpawnEnemies()
    {
        while (true) 
        {
            if (enemyCount < enemyMax)
            {
                Vector3 spawnPoint = GetSpawnPoint(spawnRadius);
                if (enemyGameObjects.Count > 0) 
                {
                    System.Random random = new System.Random();
                    int index = random.Next(enemyGameObjects.Count);
                    GameObject enemyGameObject = enemyGameObjects[index];
                    enemyGameObjects.RemoveAt(index);
                    enemyGameObject.transform.position = spawnPoint;
                    enemyGameObject.GetComponent<EnemyAI>().health = 100;
                    enemyGameObject.SetActive(true);
                } 
                else 
                {
                    SpawnRandomPrefab(spawnPoint);
                }
                enemyCount++;
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnRandomPrefab(Vector3 spawnPoint)
    {
        int randomIndex = Random.Range(0, prefabs.Count);
        GameObject selectedPrefab = prefabs[randomIndex];
        Runner.Spawn(selectedPrefab, spawnPoint, Quaternion.identity);
    }

    private Vector3 GetSpawnPoint(float radius)
    {
        if(spawnTarget != null) 
        {
            Vector3 pos = spawnTarget.transform.position;
            pos.y = 0;
            pos.z = pos.z + 2;
            return pos;
        }

        Vector3 playerPosition = GameObject.FindWithTag(TagConstants.TRAIN).transform.position;
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, radius);
        float xPos = playerPosition.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float zPos = playerPosition.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(xPos, 0, zPos);
    }
}
