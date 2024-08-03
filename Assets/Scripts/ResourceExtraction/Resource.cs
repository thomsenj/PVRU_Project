using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType resourceType;
    public int amount = 1;
    public int hitsToHarvest = 3;

    private int currentHits = 0;
    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = GameObject.FindWithTag(GeneralConstants.Player2Name).GetComponent<PlayerInventory>();
    }

    public void Harvest()
    {
        currentHits++;
        Debug.Log($"Resource hit {currentHits} times.");

        if (currentHits >= hitsToHarvest)
        {
            playerInventory.AddResource(resourceType, amount);
            Destroy(gameObject);
        }
    }
}
