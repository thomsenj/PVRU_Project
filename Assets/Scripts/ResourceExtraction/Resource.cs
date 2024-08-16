using UnityEngine;
using System.Collections;
using Fusion;

namespace Fusion.XRShared.Demo
{
    public class Resource : NetworkBehaviour
    {
        public ResourceType resourceType;
        public int amount = 1;
        public int hitsToHarvest = 3;
        public Color hitColor = Color.red;
        public float colorChangeDuration = 0.2f;

        private int currentHits = 0;
        private PlayerInventory playerInventory;
        private Renderer resourceRenderer;
        private Color originalColor;

        void Start()
        {
            GameObject player = GameObject.FindWithTag(TagConstants.Player2Name);
            playerInventory = player.GetComponent<PlayerInventory>();
            resourceRenderer = GetComponent<Renderer>();
            if (resourceRenderer != null)
            {
                originalColor = resourceRenderer.material.color;
            }
        }

        void OnTriggerEnter(Collider other)
        {

            if (other.CompareTag(TagConstants.RESOURCE_TOOL))
            {
                Harvest();
            }
        }

        public void Harvest()
        {
            currentHits++;
            Debug.Log($"Resource hit {currentHits} times.");

            if (resourceRenderer != null)
            {
                StartCoroutine(ChangeColorTemporarily());
            }

            if (currentHits >= hitsToHarvest)
            {
                if (playerInventory != null)
                {
                    playerInventory.AddResource(resourceType, amount);
                }
                Debug.Log("Should Despawn now.");
                NotifyDespawn();
            }
        }

        private IEnumerator ChangeColorTemporarily()
        {
            resourceRenderer.material.color = hitColor;
            yield return new WaitForSeconds(colorChangeDuration);
            resourceRenderer.material.color = originalColor;
        }


        private void NotifyDespawn()
        {
          ResourceSpawnerPrefab spawner = GameObject.FindWithTag(TagConstants.WORLD_MANAGER).GetComponent<ResourceSpawnerPrefab>();
          spawner.Despawn(this.gameObject);
        }
    }
}