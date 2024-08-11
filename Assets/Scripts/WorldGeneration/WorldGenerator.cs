using System.Collections.Generic;
using Dreamteck.Splines;
using Dreamteck.Splines.Examples;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldGenerator : MonoBehaviour
{
    public List<GameObject> planes;
    public GameObject changePlane;
    public float planeRadius;

    private GameObject train;
    private List<GameObject> carriages;
    private SplineFollower splineFollower;
    private SplineComputer splineComputer;
    private GameObject mainPlane;
    private GameObject lastPlane;

    private void Start()
    {
        train = GameObject.FindGameObjectWithTag(TagConstants.TRAIN);
        carriages = new List<GameObject>(GameObject.FindGameObjectsWithTag(TagConstants.CARRIAGE));
        splineFollower = train.GetComponent<SplineFollower>();
        mainPlane = planes[0];
        lastPlane = planes[planes.Count - 1];
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
                if (distance < planeRadius && plane != mainPlane)
                {
                    //SplineComputer newSpline = plane.GetComponent<SplineComputer>();
                    //if (newSpline != null)
                    //{
                    //    splineFollower.spline = newSpline;
                    //    splineFollower.SetPercent(0.0);
                    //    foreach(GameObject carriage in carriages)
                    //    {
                    //        SplinePositioner splinePositioner = carriage.GetComponent<SplinePositioner>();
                    //        splinePositioner.spline = newSpline;
                    //    }
                    //}
                    changePlane.transform.position = new Vector3(lastPlane.transform.position.x + (planeRadius * 2), lastPlane.transform.position.y, lastPlane.transform.position.z);
                    lastPlane = changePlane;
                    changePlane = mainPlane;
                    mainPlane = plane;
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

}
