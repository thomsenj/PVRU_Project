using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController characterController;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private PhysXBullet _physXBulletPrefab;

    private Vector3 _forward = Vector3.forward;

    [Networked] private TickTimer delay { get; set; }

    private void Start()
    {
        characterController = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        // Every Tick
        if (GetInput(out NetworkInputData data))
        {
            // Movement
            data.direction.Normalize();
            characterController.Move(10 * data.direction * Runner.DeltaTime);

            // Instatiate Bullet
            if (data.direction.sqrMagnitude > 0)
            {
                _forward = data.direction; // Contain direction of movement
            }

            if (HasStateAuthority && delay.ExpiredOrNotRunning(Runner))
            {
                if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON0))
                {
                    Runner.Spawn(_bulletPrefab, transform.position + _forward, Quaternion.LookRotation(_forward), Object.InputAuthority,
                    (Runner, O) =>
                    {
                        O.GetComponent<Bullet>().Init();
                    }
                    );
                }
                else if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON1))
                {
                    Runner.Spawn(_physXBulletPrefab, transform.position + _forward, Quaternion.LookRotation(_forward), Object.InputAuthority,
                    (Runner, O) =>
                    {
                        O.GetComponent<PhysXBullet>().Init(10 * _forward);
                    }
                    );
                }
            }
        }
    }
}
