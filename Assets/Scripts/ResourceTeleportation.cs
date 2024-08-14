using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceTeleportation : MonoBehaviour
{
    [SerializeField] private Transform _ResourceTarget;
    [SerializeField] private Transform _FuelTarget;
    [SerializeField] private Vector3 _ResourceTeleportOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private Vector3 _FuelTeleportOffset = new Vector3(0, 1.0f, 0);
    [SerializeField] private string[] _AdditionalResourceTags = null;
    [SerializeField] private string[] _AdditionalFuelTags = null;
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private CoalPile coalPile;

    // Start is called before the first frame update
    void Start()
    {
        GetPlayerInventory();
        GetCoalPile();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        //temporary solution
        if (IsResourceTag(other.tag))
        {
            HandleCoal();
            other.gameObject.SetActive(false);
            return;
        }
        
        if (IsResourceTag(other.tag) && _ResourceTarget != null)
        {
            TeleportObject(other.transform, _ResourceTarget.position, _ResourceTeleportOffset);
        }
        else if (IsFuelTag(other.tag) && _FuelTarget != null)
        {
            TeleportObject(other.transform, _FuelTarget.position, _FuelTeleportOffset);
        }
    }

    private void TeleportObject(Transform transformObject, Vector3 targetPosition, Vector3 offset)
    {
        transformObject.position = targetPosition + offset;

        Rigidbody rb = transformObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private bool IsResourceTag(string tag)
    {
        if (tag == "Resource")
        {
            return true;
        }

        if (_AdditionalResourceTags != null)
        {
            foreach (string resourceTag in _AdditionalResourceTags)
            {
                if (tag == resourceTag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool IsFuelTag(string tag)
    {
        if (tag == TagConstants.COAL) {
            return true;
        }

        if (_AdditionalFuelTags != null)
        {
            foreach (string fuelTag in _AdditionalFuelTags)
            {
                if (tag == fuelTag)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void GetPlayerInventory() {
        if (playerInventory == null) {
            try {
                playerInventory = GameObject.FindGameObjectWithTag(TagConstants.Player2Name).GetComponent<PlayerInventory>();
            } catch {
                Debug.LogError("Player 2 needs a Inventory. Check your components.");
            }
        }
    }

    private void GetCoalPile() {
        if (coalPile == null) {
            try {
                coalPile = GameObject.FindGameObjectWithTag(TagConstants.COAL_PILE).GetComponent<CoalPile>();
            } catch {
                Debug.LogError("Train needs a Coal pile. Check your components.");
            }
        }
    }

 private void HandleCoal() {
    Debug.Log("In Handle Coal");
    Dictionary<ResourceType, int> resources = playerInventory.getResources();
    int woodAmount = resources.ContainsKey(ResourceType.Wood) ? resources[ResourceType.Wood] : 0;
    int stoneAmount = resources.ContainsKey(ResourceType.Stone) ? resources[ResourceType.Stone] : 0;
    int maxCoalByWood = woodAmount / 2;
    int maxCoalByStone = stoneAmount;
    int amount = Mathf.Min(maxCoalByWood, maxCoalByStone);
    if(amount > 0) {
        coalPile.coalAmount += amount;
        playerInventory.RemoveResource(ResourceType.Wood, amount * 2);
        playerInventory.RemoveResource(ResourceType.Stone, amount);
        Debug.Log("Succefully added coal: " + coalPile.coalAmount);
    } 
}
}
