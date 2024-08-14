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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
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
        if (tag == "Fuel")
        {
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
}
