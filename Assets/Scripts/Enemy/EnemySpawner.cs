using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

namespace Fusion.XRShared.Demo
{
    public class EnemyPrefabSpawner : NetworkBehaviour
    {
        [SerializeField] private List<NetworkObject> prefab;
        private NetworkObject currentInstance;
        [SerializeField] public GameObject spawnTarget;

        [Networked]
        public int currentCount {get; set;}

        [SerializeField] private int maxCount;

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
                       // Spawn();
                        currentCount++;
                    }
                    spawnTimer = 0f;
                }
            }
        }


        void Spawn()
        {
            if (prefab == null || prefab.Count == 0) return;
            System.Random random = new System.Random();
            int index = random.Next(prefab.Count);
            NetworkObject enemyGameObject = prefab[index];
            currentInstance = Runner.Spawn(enemyGameObject, GetSpawnPoint(10.0f));
        }


    private Vector3 GetSpawnPoint(float radius)
    {
        if(spawnTarget != null) 
        {
            Vector3 pos = spawnTarget.transform.position;
            pos.y = 0;
            pos.z = pos.z + 2;
            return pos;
        }

        Vector3 playerPosition = GameObject.FindWithTag(TagConstants.TRAIN).transform.position;
        float angle = Random.Range(0f, 360f);
        float distance = Random.Range(0f, radius);
        float xPos = playerPosition.x + distance * Mathf.Cos(angle * Mathf.Deg2Rad);
        float zPos = playerPosition.z + distance * Mathf.Sin(angle * Mathf.Deg2Rad);
        return new Vector3(xPos, 0, zPos);
    }

        public void UpdateEnemies(List<GameObject> enemies, GameObject plane)
        {
            
            PlaneInformation planeInfo = plane.GetComponent<PlaneInformation>();
            spawnTarget = planeInfo.enemySpawner;
           
        }


        public void Despawn(GameObject gameObject) {
            Runner.Despawn(gameObject.GetComponent<NetworkObject>());
            currentCount--;
        }
    }

}
