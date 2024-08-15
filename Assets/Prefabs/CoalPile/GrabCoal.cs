using Fusion;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VRPlayerInteraction : NetworkBehaviour
{
    public string rightHandControllerName = "RightHand Controller";
    private ActionBasedController rightHandController;
    private NetworkObject heldObjectNetworked;
    private bool isGrabbing;

    void Start()
    {
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

        if (isGrabbing && heldObjectNetworked == null)
        {
            TryGrab();
        }
        else if (!isGrabbing && heldObjectNetworked != null)
        {
            DropHeldObject();
        }
    }

    void TryGrab()
    {
        if (!HasStateAuthority) return;  // Nur der Spieler mit der Zustandsautorität kann Objekte greifen

        RaycastHit hit;
        if (Physics.Raycast(rightHandController.transform.position, rightHandController.transform.forward, out hit, 5f))
        {
            NetworkObject networkObject = hit.collider.GetComponent<NetworkObject>();
            if (networkObject != null && networkObject.HasStateAuthority)
            {
                RPC_TakeOwnershipAndGrab(networkObject);
            }
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_TakeOwnershipAndGrab(NetworkObject targetObject)
    {
        targetObject.RequestStateAuthority();  // Übernimmt die Kontrolle über das Objekt
        heldObjectNetworked = targetObject;

        Rigidbody rb = heldObjectNetworked.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        heldObjectNetworked.transform.SetParent(rightHandController.transform);
        heldObjectNetworked.transform.localPosition = Vector3.zero;

        RPC_SyncGrabbedObject(heldObjectNetworked.Id);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    void RPC_SyncGrabbedObject(NetworkId objectId)
    {
        NetworkObject networkObject = Runner.FindObject(objectId);
        if (networkObject != null)
        {
            heldObjectNetworked = networkObject;
            heldObjectNetworked.transform.SetParent(rightHandController.transform);
            heldObjectNetworked.transform.localPosition = Vector3.zero;
        }
    }

    void DropHeldObject()
    {
        if (heldObjectNetworked != null)
        {
            Rigidbody rb = heldObjectNetworked.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;

                rb.velocity = rightHandController.transform.forward * 3f;
            }

            heldObjectNetworked.transform.SetParent(null);
            heldObjectNetworked = null;
        }
    }
}
