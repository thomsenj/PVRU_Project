using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class CoalPile : NetworkBehaviour
{
    [Networked]
    public int coalAmount { get; set; } = 10;

    public GameObject coalPrefab; 
    
}
