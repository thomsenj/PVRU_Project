using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public List<GameObject> prefabs;

    public GameObject spawnTarget;

    public int enemyCount = 0;
    public int enemyMax = 5;
    public float spawnRadius = 10f;
    public float spawnInterval = 4f; 

    private ScoreManager scoreManager;
    private List<GameObject> enemyGameObjects;

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ScoreManager>();
        Debug.Log(scoreManager);
        enemyGameObjects = new List<GameObject>();
        StartCoroutine(SpawnEnemies());
    }

    private void Update() {
        enemyMax = scoreManager.GetEnemyCount();
    }

    public void EnemyDied(GameObject enemy) {
        enemyGameObjects.Add(enemy);
        if(enemyCount > 0) {
            enemyCount--;
        }
    }

    IEnumerator SpawnEnemies()
    {
         while (true) {
            if (enemyCount < enemyMax)
            {
                Vector3 spawnPoint = GetSpawnPoint(spawnRadius);
                if(enemyGameObjects.Count > 0) {
                    System.Random random = new System.Random();
                    int index = random.Next(enemyGameObjects.Count);
                    GameObject enemyGameObject = enemyGameObjects[index];
                    enemyGameObjects.RemoveAt(index);
                    enemyGameObject.transform.position = spawnPoint;
                    enemyGameObject.SetActive(true);
                } else {
                    SpawnRandomPrefab(spawnPoint);
                }
                enemyCount++;
                yield return new WaitForSeconds(spawnInterval);
            }
        }
    }

    private void SpawnRandomPrefab(Vector3 spawnPoint)
    {
        int randomIndex = Random.Range(0, prefabs.Count);
        GameObject selectedPrefab = prefabs[randomIndex];
        Instantiate(selectedPrefab, spawnPoint, UnityEngine.Quaternion.identity);
    }

    private Vector3 GetSpawnPoint(float radius)
    {
        if(spawnTarget != null) {
            Vector3 pos = spawnTarget.transform.position;
            pos.y = 0;
            pos.z = pos.z + 2;
            return pos;
        }
        Vector3 playerPosition = GameObject.FindWithTag(TagConstants.Player2Name).transform.position;
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, radius);
        float xPos = playerPosition.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float zPos = playerPosition.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(xPos, 0, zPos);
    }
}
