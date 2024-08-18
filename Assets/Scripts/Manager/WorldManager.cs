using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Fusion.XRShared.Demo;
using Unity.VisualScripting;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // in the start the plane where the trains stands
    public GameObject mainPlane;
    public float planeRadius = 75f;
    public GameObject train;
    private TrainManager trainManager;
    private ResourceSpawnerPrefab ResourceSpawnerPrefab;
    private EnemyPrefabSpawner EnemyPrefabSpawner;
    private bool safetyLock = false; 

    private void Start()
    {
        train = GameObject.FindGameObjectWithTag(TagConstants.TRAIN);
        GameObject worldManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER);
        trainManager = worldManager.GetComponent<TrainManager>();
        ResourceSpawnerPrefab = worldManager.GetComponent<ResourceSpawnerPrefab>();
        EnemyPrefabSpawner = worldManager.GetComponent<EnemyPrefabSpawner>();
    }

    private void Update()
    {
        PlaneInformation planeInformation = mainPlane.GetComponent<PlaneInformation>();
        Vector3 position = train.transform.position;
        float distance = GetDistance(mainPlane, position);
        float nextDistance = GetDistance(planeInformation.nextPlane, position);
        if (distance < 60f)
        {
            safetyLock = true;
        }
        if (distance > planeRadius && safetyLock && distance > nextDistance)
        {
            safetyLock = false;
            ClearEnemies();
            mainPlane = planeInformation.nextPlane;
            UpdateEnemies();
            UpdateTrainManager();
            UpdateResourceFactory();
        }
    }

    private float GetDistance(GameObject plane, Vector3 trainPosition)
    {
        try
        {
            Vector3 direction = plane.transform.position - trainPosition;
            return direction.magnitude;
        }
        catch
        {
            return -1;
        }
    }

    private void UpdateResourceFactory()
    {
       ResourceSpawnerPrefab.UpdateResourceFactory(mainPlane);
    }

    private void UpdateEnemies()
    {
        EnemyPrefabSpawner.UpdateEnemies(mainPlane);
    }

    private void ClearEnemies()
    {
        EnemyPrefabSpawner.ClearEnemies(mainPlane);
    }

    private void UpdateTrainManager()
    {
        trainManager.weatherType = mainPlane.GetComponent<PlaneInformation>().getPlaneInfo().WeatherType;
    }
}