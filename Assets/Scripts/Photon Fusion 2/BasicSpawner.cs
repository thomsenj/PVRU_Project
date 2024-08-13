using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEditor;
using TMPro;
using UnityEngine.Rendering;
using Fusion.Addons.Physics;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner networkRunner;
    [SerializeField] private NetworkPrefabRef networkPrefabRef;

    private Dictionary<PlayerRef, NetworkObject> spawnCharacter = new Dictionary<PlayerRef, NetworkObject>();

    private bool _mouseButton0;
    private bool _mouseButton1;

    // Host 
    async void StartGame(GameMode mode)
    {
        // Creating Runner and saying user is giving input
        networkRunner = gameObject.AddComponent<NetworkRunner>();
        gameObject.AddComponent<RunnerSimulatePhysics3D>().ClientPhysicsSimulation = ClientPhysicsSimulation.SimulateForward;
        networkRunner.ProvideInput = true;

        // Collecting Scene Information
        var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        var sceneInfo = new NetworkSceneInfo();
        if (scene.IsValid)
        {
            sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        }

        // Create Session / Room
        await networkRunner.StartGame(new StartGameArgs()
        {
            GameMode = mode,
            SessionName = "TestRoom",
            Scene = scene,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        }
        );
    }

    void Update()
    {
        _mouseButton0 = _mouseButton0 | Input.GetMouseButtonDown(0);
        _mouseButton1 = _mouseButton0 | Input.GetMouseButtonDown(1);
    }

    private void OnGUI()
    {
        if (networkRunner == null)
        {
            if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
            {
                StartGame(GameMode.Host);
            }
            if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
            {
                StartGame(GameMode.Client);
            }
        }
    }

    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner)
    {
        // throw new NotImplementedException();
    }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
    {
        // throw new NotImplementedException();
    }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
    {
        // throw new NotImplementedException();
    }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
    {
        // throw new NotImplementedException();
    }

    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
    {
        // throw new NotImplementedException();
    }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
    {
        // throw new NotImplementedException();
    }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        // throw new NotImplementedException();
        var data = new NetworkInputData();

        if (Input.GetKey(KeyCode.W))
        {
            data.direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            data.direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            data.direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            data.direction += Vector3.right;
        }

        // Add structure & Host
        data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _mouseButton0);
        _mouseButton0 = false;

        data.buttons.Set(NetworkInputData.MOUSEBUTTON1, _mouseButton1);
        _mouseButton1 = false;

        input.Set(data); // Passing to host
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
    {
        // throw new NotImplementedException();
    }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        // throw new NotImplementedException();
    }

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
    {
        // throw new NotImplementedException();
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        // throw new NotImplementedException();
        if (runner.IsServer)
        {
            Vector3 playerPos = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 1f, 0f);

            NetworkObject networkObject = runner.Spawn(networkPrefabRef, playerPos, Quaternion.identity, player);

            spawnCharacter.Add(player, networkObject);
        }
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        // throw new NotImplementedException();
        if (spawnCharacter.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            spawnCharacter.Remove(player);
        }
    }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
    {
        // throw new NotImplementedException();
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
    {
        // throw new NotImplementedException();
    }

    public void OnSceneLoadDone(NetworkRunner runner)
    {
        // throw new NotImplementedException();
    }

    public void OnSceneLoadStart(NetworkRunner runner)
    {
        // throw new NotImplementedException();
    }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
    {
        // throw new NotImplementedException();
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
    {
        // throw new NotImplementedException();
    }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
    {
        // throw new NotImplementedException();
    }
}
