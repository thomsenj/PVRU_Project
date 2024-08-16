using System.Collections.Generic;
using Fusion.XRShared.Demo;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private CollectableBankController woodController;
    private CollectableBankController stoneController;
    private Dictionary<ResourceType, int> resources = new Dictionary<ResourceType, int>();
    public AddCoal addCoal;

    void Start()
    {
        try
        {
            woodController = GameObject.Find(GeneralConstants.WOOD_COUNTER).GetComponent<CollectableBankController>();
            stoneController = GameObject.Find(GeneralConstants.STONE_COUNTER).GetComponent<CollectableBankController>();
        }
        catch
        {
            Debug.LogError("Missing Player Inventory UI.");
        }
        foreach (ResourceType resourceType in System.Enum.GetValues(typeof(ResourceType)))
        {
            resources[resourceType] = 0;
            SetUI(resourceType, resources[resourceType]);
        }
    }
    void Update()
    {
        bool coal = HandleCoal();
        if (coal)
        {
            addCoal.spawnCoal();
        }
    }

    public Dictionary<ResourceType, int> getResources()
    {
        return resources;
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

    public void RemoveResource(ResourceType resourceType, int amount)
    {
        if (resources.ContainsKey(resourceType))
        {
            resources[resourceType] -= amount;
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

    private void SetUI(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Wood:
                if (woodController != null)
                {
                    woodController.SetCount(amount);
                    Debug.Log("Set Wood");
                }
                break;
            case ResourceType.Stone:
                if (stoneController != null)
                {
                    stoneController.SetCount(amount);
                    Debug.Log("Set Stone: " + amount);
                }
                break;
            default:
                throw new InvalidResourceTypeException("Invalid Resource Type. How Could That Happen!");
        }
    }

    private bool HandleCoal()
    {
        int woodAmount = resources[ResourceType.Wood];
        int stoneAmount = resources[ResourceType.Stone];
        int maxCoalByWood = woodAmount / 2;
        int maxCoalByStone = stoneAmount;
        if(maxCoalByWood == 0 || maxCoalByStone == 0) {
            return false;
        }
        int amount = Mathf.Min(maxCoalByWood, maxCoalByStone);
        if (amount > 0)
        {
            resources[ResourceType.Wood] = resources[ResourceType.Wood] - 2; 
            resources[ResourceType.Stone] = resources[ResourceType.Stone] - 1;
            return true;
        }
        return false;
    }
}
