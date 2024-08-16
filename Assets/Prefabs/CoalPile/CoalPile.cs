using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class CoalPile : NetworkBehaviour
{
    [Networked]
    public int coalAmount { get; set; } = 10;

    public GameObject coalPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagConstants.COAL_PILE))
        {
            if (HasStateAuthority)
            {
                SpawnCoalInHand(other.gameObject.transform);
            }
        }
    }

    private void SpawnCoalInHand(Transform hand)
    {
        if (coalPrefab == null)
        {
            Debug.LogWarning("Coal Prefab ist nicht zugewiesen.");
            return;
        }

        Vector3 spawnPosition = hand.position;
        Quaternion spawnRotation = hand.rotation;

        NetworkObject coalObject = Runner.Spawn(coalPrefab, spawnPosition, spawnRotation);

        coalObject.transform.SetParent(hand);
    }

}
