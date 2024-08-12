using System;
using System.Collections.Generic;
using Dreamteck.Splines;
using Dreamteck.Splines.Examples;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class WorldGenerator : MonoBehaviour
{
    public List<GameObject> planes;
    public GameObject changePlane;
    public float planeRadius;

    private GameObject train;
    private PlanePool planePool;
    private GameObject mainPlane;
    private GameObject lastPlane;
    private int lastNodeCounter;
    private int nextNodeIndex;
    bool updateLock;

    private void Start()
    {
        planePool = GameObject.FindGameObjectWithTag(TagConstants.PLANE_POOL).GetComponent<PlanePool>();
        train = GameObject.FindGameObjectWithTag(TagConstants.TRAIN);
        mainPlane = planes[0];
        lastPlane = planes[planes.Count - 1];
        lastNodeCounter = 0;
        nextNodeIndex = 3;
        updateLock = false;
    }

    private void Update()
    {
        if (train == null || planes == null || planes.Count == 0)
        {
            Debug.LogError("Train oder planes sind nicht richtig initialisiert.");
            return;
        }

        Vector3 position = train.transform.position;

        foreach (GameObject plane in planes)
        {
            if (plane != null)
            {
                float distance = GetDistance(plane, position);

                if(distance < planeRadius/2 && plane == mainPlane && updateLock) {
                    updateLock = false;
                }
                
                if (distance < planeRadius && plane != mainPlane && !updateLock)
                {
                    // handle nodes
                    RemoveNode();
                    GameObject newLastPlane = planePool.GetAndRemoveRandomGameObject();
                    Vector3 newPosition = new Vector3(
                        lastPlane.transform.position.x + (planeRadius * 2), 
                        lastPlane.transform.position.y, 
                        lastPlane.transform.position.z
                    );  
                    newLastPlane.transform.position = newPosition;           
                    AddNode(newLastPlane);

                    // handle planes
                    lastPlane = newLastPlane;
                    lastPlane.SetActive(true);
                    changePlane.SetActive(false);
                    planePool.AddPlane(changePlane);
                    changePlane = mainPlane;
                    mainPlane = plane;
                    updateLock = true;
                }
            }
            else
            {
                Debug.LogWarning("Ein Plane-GameObject in der Liste ist null.");
            }
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

    private void AddNode(GameObject incomingPlane)
    {
        // Find the junction controller by tag
        GameObject junctions = GameObject.FindGameObjectWithTag(TagConstants.JUNCTIION_CONTROLLER);

        // Handle the case where the junction controller is missing
        if (junctions == null)
        {
            Debug.LogError("Missing JunctionController");
            return; // Exit early if the junction controller is not found
        }

        // Get the spline computers and points
        SplineComputer changePlaneSplineComputer = incomingPlane.GetComponent<SplineComputer>();
        SplineComputer lastPlaneSplineComputer = lastPlane.GetComponent<SplineComputer>(); 

        // Create a new GameObject for the node
        GameObject newNode = new GameObject("Node " + nextNodeIndex);
        nextNodeIndex++;

        // Add the Node component to the new GameObject
        Node node = newNode.AddComponent<Node>();

        // Use the AddConnection method to add connections to the node
        SplinePoint[] points = lastPlaneSplineComputer.GetPoints();
        node.transform.position = points[points.Length - 1].position;
        node.AddConnection(lastPlaneSplineComputer, lastPlaneSplineComputer.pointCount - 1);
        node.AddConnection(changePlaneSplineComputer, 0);

        // Create and configure the JunctionSwitch.Bridge
        JunctionSwitch.Bridge bridge = new JunctionSwitch.Bridge
        {
            a = 0,
            aDirection = JunctionSwitch.Bridge.Direction.Forward,
            b = 1,
            bDirection = JunctionSwitch.Bridge.Direction.Forward
        };

        // Add the JunctionSwitch component to the new GameObject and set up the bridge
        JunctionSwitch junctionSwitch = newNode.AddComponent<JunctionSwitch>();
        junctionSwitch.bridges = new JunctionSwitch.Bridge[] { bridge };

        // Parent the new node to the junctions GameObject
        newNode.transform.SetParent(junctions.transform);

        // Log success
        Debug.Log("New node added successfully to the junctions controller");
    }

    private void RemoveNode()
    {
        try
        {
            GameObject nodeToRemove = GameObject.Find("Node " + lastNodeCounter);
            Destroy(nodeToRemove);
        } catch
        {
            Debug.LogError("Could not remove node");
        }
        lastNodeCounter++;
    }
}
