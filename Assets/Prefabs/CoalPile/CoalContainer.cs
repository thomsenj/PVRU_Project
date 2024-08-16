using System.Linq;
using System.Collections;
using UnityEngine;
using Fusion;
using Fusion.XRShared.Demo;

public class CoalContainer : NetworkBehaviour
{
    private AddCoal addCoal;
    private ResourceSpawnerPrefab resourceSpawnerPrefab;
    private int currentHits;
    [SerializeField] private int maxHitsToKill;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (maxHitsToKill == currentHits) {
                AddCoal addCoal = GameObject.FindGameObjectWithTag(TagConstants.COAL_PILE).GetComponent<AddCoal>();
                ResourceSpawnerPrefab resourceSpawnerPrefab = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ResourceSpawnerPrefab>();
                addCoal.spawnCoal();
                resourceSpawnerPrefab.Despawn(gameObject);
            } else {
                currentHits++;
            }
        }
    }
}