using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    // Movement
    public Vector3 direction;

    // Bullet function
    public const byte MOUSEBUTTON0 = 1;
    public const byte MOUSEBUTTON1 = 2;
    public NetworkButtons buttons;
}
