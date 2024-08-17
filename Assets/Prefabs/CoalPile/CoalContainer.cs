using UnityEngine;
using Fusion;
using Fusion.XRShared.Demo;

public class CoalContainer : NetworkBehaviour
{
    private ResourceSpawnerPrefab resourceSpawnerPrefab;
    private int currentHits = 0;
    [SerializeField] private int maxHitsToKill;
    [SerializeField] private ResourceContainer resourceContainer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (maxHitsToKill == currentHits)
            {
                HandleSpawn();
                resourceSpawnerPrefab = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ResourceSpawnerPrefab>();
                resourceSpawnerPrefab.Despawn(gameObject);
            }
            else
            {
                currentHits++;
            }
        }
    }

    private void HandleSpawn()
    {
        if(resourceContainer == ResourceContainer.COAL)
        {
            AddCoal addCoal = GameObject.FindGameObjectWithTag(TagConstants.COAL_PILE).GetComponent<AddCoal>();
            addCoal.spawnCoal();
        }
        if (resourceContainer == ResourceContainer.WATER)
        {
            WaterBucket w = GameObject.FindGameObjectWithTag(TagConstants.RESOURCE_TOOL).GetComponent<WaterBucket>();
            w.FillUp();
        }
    }
}

public enum ResourceContainer
{
    COAL,
    WATER
}