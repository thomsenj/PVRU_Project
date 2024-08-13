using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceFactory : MonoBehaviour
{
    public List<GameObject> prefabs;

    public GameObject spawnTarget;

    public int resourceCount = 0;
    public int resourceMax = 5;
    public float spawnRadius = 50f;
    public float spawnInterval = 4f; 

    private List<GameObject> resourceGameObjects;

    private void Start()
    {
        resourceGameObjects = new List<GameObject>();
        StartCoroutine(SpawnResources());
    }

    public int getLastPlaneInfo() {
        return resourceCount;
    }

    public void UpdateResourceFactory(GameObject plane) {
        PlaneInfo planeInfo = plane.GetComponent<PlaneInformation>().getPlaneInfo();
        spawnTarget = plane;
        resourceCount = planeInfo.ResourceCount;
        resourceMax = planeInfo.ResourceMax;
        prefabs = planeInfo.ResourcePrefabs;
    }

    IEnumerator SpawnResources()
    {
         while (true) {
            if (resourceCount < resourceMax)
            {
                Vector3 spawnPoint = GetSpawnPoint(spawnRadius);
                if(resourceGameObjects.Count > 0) {
                    System.Random random = new System.Random();
                    int index = random.Next(resourceGameObjects.Count);
                    GameObject enemyGameObject = resourceGameObjects[index];
                    resourceGameObjects.RemoveAt(index);
                    enemyGameObject.transform.position = spawnPoint;
                    enemyGameObject.SetActive(true);
                } else {
                    SpawnRandomPrefab(spawnPoint);
                }
                resourceCount++;
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
        Vector3 pos = spawnTarget.transform.position;
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, radius);
        float xPos = pos.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float zPos = pos.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(xPos, 0, zPos);
    }
}
