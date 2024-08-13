using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using Unity.VisualScripting;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // in the start the plane where the trains stands
    public GameObject mainPlane;
    public float planeRadius = 75f;
    public GameObject train;
    private ResourceFactory resourceFactory;
    private EnemyFactory enemyFactory;

    private void Start()
    {
        train = GameObject.FindGameObjectWithTag(TagConstants.TRAIN);
        GameObject worldManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER);
        resourceFactory =  worldManager.GetComponent<ResourceFactory>();
        enemyFactory = worldManager.GetComponent<EnemyFactory>();
    }

     private void Update()
    {
        if (train == null || mainPlane == null)
        {
            Debug.LogError("Train oder Mainplane not initialized.");
            return;
        }

        Vector3 position = train.transform.position;
        float distance = GetDistance(mainPlane, position);
        if(distance > planeRadius) {
            int resourceCount = resourceFactory.getLastPlaneInfo();
            PlaneInformation planeInformation = mainPlane.GetComponent<PlaneInformation>();
            planeInformation.resourceCount = resourceCount;
            ClearEnemies();
            mainPlane = planeInformation.nextPlane;
            UpdateEnemyFactory();
            UpdateResourceFactory();
            Update
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

    private void UpdateResourceFactory() {
        resourceFactory.UpdateResourceFactory(mainPlane);
    }

    private void ClearEnemies() {
        List<GameObject> enemies = new List<GameObject>(GameObject.FindGameObjectsWithTag(TagConstants.Enemy));
        enemyFactory.UpdateEnemies(enemies);
    }

    private void UpdateEnemyFactory() {
        GameObject spawner = mainPlane.GetComponent<PlaneInformation>().enemySpawner;
        enemyFactory.ResetFactory(spawner);
    }
}