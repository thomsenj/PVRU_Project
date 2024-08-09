using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerInteraction : MonoBehaviour
{
    public string rightHandControllerName = "RightHand Controller";
    private ActionBasedController rightHandController;
    private GameObject heldObject;
    private bool isGrabbing;

    void Start()
    {
        // Versuche, den RightHand Controller zu finden
        GameObject rightHandControllerObject = GameObject.Find(rightHandControllerName);
        if (rightHandControllerObject != null)
        {
            rightHandController = rightHandControllerObject.GetComponent<ActionBasedController>();
            if (rightHandController == null)
            {
                Debug.LogError("XRController component not found on the RightHand Controller GameObject.");
            }
        }
        else
        {
            Debug.LogError("RightHand Controller GameObject not found in the scene.");
        }
    }

    void Update()
    {
        if (rightHandController == null)
        {
            Debug.LogError("RightHandController reference is not set in the Inspector.");
            return;
        }

        isGrabbing = rightHandController.selectAction.action.ReadValue<float>() > 0.1f;

        if (isGrabbing && heldObject == null)
        {
            TryGrab();
        }
        else if (!isGrabbing && heldObject != null)
        {
            DropHeldObject();
        }
    }

    void TryGrab()
    {
        RaycastHit hit;
        if (Physics.Raycast(rightHandController.transform.position, rightHandController.transform.forward, out hit, 5f))
        {
            CoalPile coalPile = hit.collider.GetComponent<CoalPile>();
            if (coalPile != null)
            {
                GameObject coal = coalPile.TakeCoal();
                if (coal != null)
                {
                    Rigidbody rb = coal.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }

                    coal.transform.SetParent(rightHandController.transform);
                    coal.tag = TagConstants.COAL;
                    coal.transform.localPosition = Vector3.zero;

                    heldObject = coal;
                }
            }
        }
    }

    void DropHeldObject()
    {
        if (heldObject != null)
        {
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;

                rb.velocity = rightHandController.transform.forward * 3f;
            }
            heldObject.transform.SetParent(null);
            heldObject = null;
        }
    }
}
