using System.Collections;
using System.Collections.Generic;
using Fusion;
using Fusion.XR.Shared.Rig;
using UnityEngine;

public class KeepPlayerRigOnTrain : NetworkBehaviour
{
    private HardwareRig hardwareRig;

    private void Awake()
    {
        hardwareRig = GetComponent<HardwareRig>();
    }

    public override void FixedUpdateNetwork()
    {
        // if (hardwareRig.trainTransform != null)
        // {
        //     hardwareRig.UpdateRigPosition();
        // }
        hardwareRig.UpdateRigPosition();
    }
}
