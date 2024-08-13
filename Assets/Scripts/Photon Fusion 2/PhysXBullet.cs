using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class PhysXBullet : NetworkBehaviour
{
    [Networked] private TickTimer life { get; set; }

    public void Init(Vector3 forward)
    {
        life = TickTimer.CreateFromSeconds(Runner, 5f);
        GetComponent<Rigidbody>().velocity = forward;
    }

    public override void FixedUpdateNetwork()
    {
        if (life.Expired(Runner))
        {
            Runner.Despawn(Object);
        }
        else
        {
            transform.position += 5 * transform.forward * Runner.DeltaTime;
        }
    }
}
