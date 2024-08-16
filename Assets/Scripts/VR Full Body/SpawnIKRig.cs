using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class SpawnIKRig : NetworkBehaviour
{
    [SerializeField] private GameObject characterPrefab;
    private NetworkObject characterInstance;
    // private NetworkRunner runner;

    public override void Spawned()
    {
        if (characterPrefab != null)
        {
            // characterInstance = Instantiate(characterPrefab, transform.position, transform.rotation);
            characterInstance = Runner.Spawn(characterPrefab, transform.position, transform.rotation, Object.InputAuthority);
        }
        AssignVRTargets();
    }

    private void AssignVRTargets()
    {
        // GameObject characterGameObject = characterInstance.gameObject;

        Transform headVRTarget = characterInstance.transform.Find("Head VR Target");
        Transform leftHandVRTarget = characterInstance.transform.Find("Left Hand VR Target");
        Transform rightHandVRTarget = characterInstance.transform.Find("Right Hand VR Target");

        var ikTargetFollowVRRig = GetComponent<IKTargetFollowVRRig>();
        if (ikTargetFollowVRRig != null)
        {
            ikTargetFollowVRRig.head.vrTarget = headVRTarget;
            ikTargetFollowVRRig.leftHand.vrTarget = leftHandVRTarget;
            ikTargetFollowVRRig.rightHand.vrTarget = rightHandVRTarget;
        }
    }
}
