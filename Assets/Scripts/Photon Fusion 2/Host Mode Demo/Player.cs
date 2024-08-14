using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class Player : NetworkBehaviour
{
    private NetworkCharacterController characterController;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private PhysXBullet _physXBulletPrefab;

    private Vector3 _forward = Vector3.forward;

    [Networked] private TickTimer delay { get; set; }

    [Networked] private bool spawnedProjectile { get; set; }

    private ChangeDetector _ChangeDetector;

    private Material _material;

    public TMP_Text messageText;

    private void Update()
    {
        if (Object.HasInputAuthority && Input.GetKeyDown(KeyCode.R))
        {
            RPC_SendMessage("Hello");
        }
    }


    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority, HostMode = RpcHostMode.SourceIsHostPlayer)]
    private void RPC_SendMessage(string message, RpcInfo rpcInfo = default)
    {
        // throw new NotImplementedException();
        RPC_RelayMessage(message, rpcInfo.Source);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsServer)]
    private void RPC_RelayMessage(string message, PlayerRef messageSource)
    {
        // throw new NotImplementedException();
        if (messageText == null)
        {
            messageText = FindObjectOfType<TMP_Text>();
        }

        if (messageSource == Runner.LocalPlayer)
        {
            message = $"You said: {message}\n";
        }
        else
        {

            message = $"Other said: {message}\n";
        }
        messageText.text += message;
    }

    private void Awake()
    {
        characterController = GetComponent<NetworkCharacterController>();
        _material = GetComponentInChildren<MeshRenderer>().material;
    }

    public override void Spawned()
    {
        _ChangeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
    }

    public override void Render()
    {
        foreach (var changes in _ChangeDetector.DetectChanges(this))
        {
            switch (changes)
            {
                case nameof(spawnedProjectile):
                    _material.color = Color.white;
                    break;
            }
        }
        _material.color = Color.Lerp(_material.color, Color.blue, Time.deltaTime);
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
                    spawnedProjectile = !spawnedProjectile;

                }
                else if (data.buttons.IsSet(NetworkInputData.MOUSEBUTTON1))
                {
                    Runner.Spawn(_physXBulletPrefab, transform.position + _forward, Quaternion.LookRotation(_forward), Object.InputAuthority,
                    (Runner, O) =>
                    {
                        O.GetComponent<PhysXBullet>().Init(10 * _forward);
                    }
                    );
                    spawnedProjectile = !spawnedProjectile;
                }
            }
        }
    }
}
