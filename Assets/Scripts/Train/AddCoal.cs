using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

namespace Fusion.XRShared.Demo
{
    public class AddCoal : NetworkBehaviour
    {
        public NetworkObject prefab;
        public Transform targetSpawn;

        public void spawnCoal() {
           NetworkObject coal = Runner.Spawn(prefab, transform.position);
           coal.transform.SetParent(targetSpawn);
        }

        public void DespawnCoal(NetworkObject networkObject) {
           Runner.Despawn(networkObject);
        }
    }
}
