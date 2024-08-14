using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour
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
    private ResourceFactory factory;

    void Start()
    {
        GameObject player = GameObject.FindWithTag(TagConstants.Player2Name);
        try {
            factory = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ResourceFactory>();
        } catch {
            Debug.Log("Add Worldmanager and or ResourceFactory to scene.");
        }
        playerInventory = player.GetComponent<PlayerInventory>();
        resourceRenderer = GetComponent<Renderer>();
        if (resourceRenderer != null)
        {
            originalColor = resourceRenderer.material.color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            if (other.CompareTag(TagConstants.RESOURCE_TOOL))
            {
                //if (pickaxe.getIsSwinging())
                //{
                    Debug.Log("Start Harvest.");
                    Harvest();
                //}
            }
        }
        catch
        {
            Debug.LogError("An error occurred during trigger handling.");
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
            playerInventory.AddResource(resourceType, amount);
            gameObject.SetActive(false);
            currentHits = hitsToHarvest;
            factory.AddResourceToList(gameObject);
        }
    }

    private IEnumerator ChangeColorTemporarily()
    {
        resourceRenderer.material.color = hitColor;
        yield return new WaitForSeconds(colorChangeDuration);
        resourceRenderer.material.color = originalColor;
    }

}
