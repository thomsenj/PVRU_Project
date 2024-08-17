using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Fusion.XRShared.Demo
{
    public class ResourceSpawnerPrefab : NetworkBehaviour
    {
        [SerializeField] private List<NetworkObject> prefab;
        [SerializeField] public GameObject spawnTarget;

        [Networked]
        public int currentCount { get; set; }

        [SerializeField] private int maxCount;

        [SerializeField] private AddCoal addCoal;

        [SerializeField] private float spawnInterval = 5.0f; // seconds
        private float spawnTimer;

        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

            if (Object.HasStateAuthority)
            {
                spawnTimer += Time.fixedDeltaTime;

                if (spawnTimer >= spawnInterval)
                {
                    if (currentCount < maxCount)
                    {
                        Vector3 pos = GameObject.FindGameObjectWithTag(TagConstants.TRAIN).transform.position;
                        Spawn(pos);
                        currentCount++;
                    }
                    spawnTimer = 0f;
                }
            }
        }

        void Spawn(Vector3 pos)
        {
            if (prefab == null || prefab.Count == 0) return;
            System.Random rand = new System.Random();
            int index = rand.Next(0, 2);
            NetworkObject resourceGameObject = prefab[index];
            Runner.Spawn(resourceGameObject, GetSpawnPoint(pos));
        }

    private Vector3 GetSpawnPoint(Vector3 pos)
    {
        Vector3 forwardDirection = spawnTarget.transform.forward;

        float angleOffset = Random.value < 0.5f ? -45f : 45f;

        float angleRad = angleOffset * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(
            forwardDirection.x * Mathf.Cos(angleRad) - forwardDirection.z * Mathf.Sin(angleRad),
            0,
            forwardDirection.x * Mathf.Sin(angleRad) + forwardDirection.z * Mathf.Cos(angleRad)
        );

        offset = offset.normalized * 7f;

        Vector3 spawnPoint = pos + offset;

        return new Vector3(spawnPoint.x, 0, spawnPoint.z);
    }


        public void UpdateResourceFactory(GameObject plane)
        {
            currentCount = 0;
        }

        public void Despawn(GameObject resource)
        {
            Runner.Despawn(resource.GetComponent<NetworkObject>());
            currentCount--;
        }

    }
}
