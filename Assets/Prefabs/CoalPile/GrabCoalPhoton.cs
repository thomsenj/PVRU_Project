using System.Collections;
using System.Collections.Generic;
using Fusion;
using System.Globalization;
using UnityEngine;

public class GrabCoalPhoton : NetworkBehaviour
{
    [SerializeField] private GameObject coalPrefab; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagConstants.COAL_PILE))
        {
            if (HasStateAuthority)
            {
                SpawnCoalInHand();
            }
        }
    }

    private void SpawnCoalInHand()
    {
        if (coalPrefab == null)
        {
            Debug.LogWarning("Coal Prefab ist nicht zugewiesen.");
            return;
        }

        Vector3 spawnPosition = transform.position; // Beispielposition, die du nach Bedarf ändern kannst
        Quaternion spawnRotation = transform.rotation; // Beispielrotation, die du nach Bedarf ändern kannst

        NetworkObject coalObject = Runner.Spawn(coalPrefab, spawnPosition, spawnRotation);

        coalObject.transform.SetParent(transform);
    }
}
