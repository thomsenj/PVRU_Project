using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    // TODO Add update rescource manager
    // TODO Add update enemy factory

    // in the start the plane where the trains stands
    public GameObject mainPlane;
    public float planeRadius = 75f;
    public GameObject train;
    private ResourceFactory resourceFactory;

    private void Start()
    {
        train = GameObject.FindGameObjectWithTag(TagConstants.TRAIN);
        resourceFactory =  GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ResourceFactory>();
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
            mainPlane.GetComponent<PlaneInformation>().resourceCount = resourceCount;
            mainPlane = mainPlane.GetComponent<PlaneInformation>().nextPlane;
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

    private void UpdateResourceFactory() {
        resourceFactory.UpdateResourceFactory(mainPlane);
    }

}