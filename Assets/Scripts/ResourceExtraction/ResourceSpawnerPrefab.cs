using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Fusion.XRShared.Demo
{
    public class ResourceSpawnerPrefab : NetworkBehaviour
    {
        [SerializeField] private List<NetworkObject> prefab;
        private NetworkObject currentInstance;
        [SerializeField] public GameObject spawnTarget;

        [Networked]
        public int currentCount { get; set; }

        [SerializeField] private int maxCount;

        [SerializeField] private float radius = 10.0f;


        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();
            if (Object.HasStateAuthority)
            {
                if (currentCount < maxCount)
                {
                    Spawn();
                    currentCount++;
                }
            }
        }

        void Spawn()
        {
            if (prefab == null || prefab.Count == 0) return;
            System.Random random = new System.Random();
            int index = random.Next(prefab.Count);
            NetworkObject resourceGameObject = prefab[index];
            currentInstance = Runner.Spawn(resourceGameObject, GetSpawnPoint());
        }

        private Vector3 GetSpawnPoint()
        {
            Vector3 pos = spawnTarget.transform.position;
            float angle = Random.Range(0f, 360f);
            float distance = Random.Range(0f, radius);
            float xPos = pos.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
            float zPos = pos.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Vector3(xPos, 0, zPos);
        }
    }
}
