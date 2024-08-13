using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public List<GameObject> enemyGameObjects;
    public int enemyCount = 0;
    public int enemyMax = 5;
    public float spawnRadius = 10f;
    public float spawnInterval = 3f; 

    private ScoreManager scoreManager;

    private void Start()
    {
        scoreManager = GetComponent<ScoreManager>();
        StartCoroutine(SpawnEnemies());
    }

    private void Update() {
        enemyMax = scoreManager.GetEnemyCount();
    }

    public void decrementEnemyCount() {
        enemyCount--;
    }

    IEnumerator SpawnEnemies()
    {
        while (enemyCount < enemyMax)
        {
            System.Random random = new System.Random();
            GameObject enemyGameObject = enemyGameObjects[random.Next(enemyGameObjects.Count)];
            Vector3 spawnPoint = GetRandomPositionAroundPlayer(spawnRadius);
            Instantiate(enemyGameObject, spawnPoint, Quaternion.identity);
            enemyCount++;
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private Vector3 GetRandomPositionAroundPlayer(float radius)
    {
        Vector3 playerPosition = GameObject.FindWithTag(TagConstants.Player2Name).transform.position;
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, radius);
        float xPos = playerPosition.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float zPos = playerPosition.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(xPos, 0, zPos);
    }
}
