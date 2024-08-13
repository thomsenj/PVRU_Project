using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private CollectableBankController woodController;
    private CollectableBankController stoneController;
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();

    void Start()
    {
        woodController = GameObject.Find(GeneralConstants.WOOD_COUNTER).GetComponent<CollectableBankController>();
        stoneController = GameObject.Find(GeneralConstants.STONE_COUNTER).GetComponent<CollectableBankController>();
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            resources[resourceType] = 0;
            SetUI(resourceType, 0);
        }
    }

    public void AddResource(ResourceType resourceType, int amount)
    {
        if (resources.ContainsKey(resourceType))
        {
            resources[resourceType] += amount;
        }
        else
        {
            resources[resourceType] = amount;
        }
        SetUI(resourceType, amount);
        Debug.Log($"{resourceType}: {resources[resourceType]}");
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        if (resources.ContainsKey(resourceType))
        {
            return resources[resourceType];
        }
        return 0;
    }

    private void SetUI(ResourceType type, int amount) {
        switch(type) {
            case ResourceType.Wood:
                woodController.SetCount(amount);
                Debug.Log("Set Wood");
                break;
            case ResourceType.Stone:
                stoneController.SetCount(amount);
                Debug.Log("Set Stone: " + amount);
                break;
            default:
                throw new InvalidResourceTypeException("Invalid Resource Type. How Could That Happen!");
        }
    } 
}
