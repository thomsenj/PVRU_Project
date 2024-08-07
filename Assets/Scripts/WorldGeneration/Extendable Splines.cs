using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;
using System.Collections.Generic;
using System;

public class ExtendableSpline : MonoBehaviour
{
    // Spline Extension
    public SplineContainer splineContainer;  
    public float addPointInterval = 5f;   
    // default value should be the length of one plane 
    public float extensionDistance = 200f;    

    // Plane Handling
    public List<GameObject> planes;
    public GameObject changePlane;

    private GameObject train;
    private GameObject mainPlane;
    private GameObject lastPlane;
    private float planeRadius;    
   
    void Start()
    {
        if (splineContainer == null)
        {
            splineContainer = GetComponent<SplineContainer>();
        }
        train = GameObject.FindGameObjectWithTag(TagConstants.TRAIN);
        mainPlane = planes[0];
        lastPlane = planes[planes.Count - 1];
        planeRadius = Math.Abs(extensionDistance) / 2;
    }

    void Update()
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
                Debug.Log(planeRadius);
                if (distance < planeRadius && plane != mainPlane)
                {
                    changePlane.transform.position = new Vector3(lastPlane.transform.position.x + (planeRadius * -2), lastPlane.transform.position.y, lastPlane.transform.position.z);
                    lastPlane = changePlane;
                    changePlane = mainPlane;
                    mainPlane = plane;
                    AddControlPoint();
                }
            }
            else
            {
                Debug.LogWarning("Ein Plane-GameObject in der Liste ist null.");
            }
        }
    }

    void AddControlPoint()
    {
        int pointCount = splineContainer.Spline.Count;
        if (pointCount == 0) return;

        BezierKnot lastPoint = splineContainer.Spline[pointCount - 1];

        Vector3 lastPosition = (Vector3)lastPoint.Position;
        // here to add dynamic track via z coordinate
        Vector3 newPointPosition = new Vector3(lastPosition.x + extensionDistance, lastPosition.y, lastPosition.z);

        var newPoint = new BezierKnot(newPointPosition);

        splineContainer.Spline.Add(newPoint);
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
}
