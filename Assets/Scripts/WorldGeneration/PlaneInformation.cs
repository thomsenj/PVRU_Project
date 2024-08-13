using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneInformation : MonoBehaviour
{

    public GameObject nextPlane;
    public GameObject enemySpawner;
    public List<GameObject> resourcePrefabs;
    public int resourceCount;
    public int resourceMax;
    public WeatherTypes weatherType;
    public PlaneInfo getPlaneInfo() {
        return new PlaneInfo(resourcePrefabs, resourceCount, resourceMax, weatherType);
    }
}

public struct PlaneInfo
{
    public List<GameObject> ResourcePrefabs { get; }
    public int ResourceCount { get; }
    public int ResourceMax { get; }
    public WeatherTypes WeatherType { get; }

    public PlaneInfo(List<GameObject> resourcePrefabs, int resourceCount, int resourceMax, WeatherTypes weatherType)
    {
        ResourcePrefabs = resourcePrefabs;
        ResourceCount = resourceCount;
        ResourceMax = resourceMax;
        WeatherType = weatherType;
    }
}


