using UnityEngine;
using System.Collections;
using Fusion;

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
            Harvest(other.gameObject.GetComponent<NetworkObject>());
        }
    }

    public void Harvest(NetworkObject networkObject)
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
            Runner.Despawn(networkObject);
        }
    }

    private IEnumerator ChangeColorTemporarily()
    {
        resourceRenderer.material.color = hitColor;
        yield return new WaitForSeconds(colorChangeDuration);
        resourceRenderer.material.color = originalColor;
    }

}
