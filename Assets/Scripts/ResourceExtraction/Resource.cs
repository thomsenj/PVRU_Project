using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType resourceType; 
    public int amount = 1; 

    private PlayerInventory playerInventory;

    void Start()
    {
        playerInventory = GameObject.FindWithTag(GeneralConstants.Player2Name).GetComponent<PlayerInventory>();
    }

    public void Harvest()
    {
        playerInventory.AddResource(resourceType, amount);
        Destroy(gameObject);
    }
}
