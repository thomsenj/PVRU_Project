using UnityEngine;
using Fusion;
using Fusion.XRShared.Demo;
using System.Collections;

public class CoalContainer : NetworkBehaviour
{
    private ResourceSpawnerPrefab resourceSpawnerPrefab;
    private int currentHits = 0;
    [SerializeField] private int maxHitsToKill;
    [SerializeField] private ResourceContainer resourceContainer;
    [SerializeField] private ParticleSystem particleSystemPrefab;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (maxHitsToKill == currentHits)
            {
                HandleSpawn();
                StartCoroutine(DestroyAfterEffect());
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

    private IEnumerator DestroyAfterEffect()
    {
        particleSystemPrefab.Play();
        yield return new WaitForSeconds(particleSystemPrefab.main.duration);
        resourceSpawnerPrefab = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ResourceSpawnerPrefab>();
        resourceSpawnerPrefab.Despawn(gameObject);
    }
}

public enum ResourceContainer
{
    COAL,
    WATER
}