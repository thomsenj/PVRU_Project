using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WorldGenerator : MonoBehaviour
{
    public List<GameObject> planes;
    public float planeRadius;

    private GameObject train;
    private GameObject mainPlane;
    private GameObject lastPlane;

    private void Start()
    {
        train = GameObject.FindGameObjectWithTag(TagConstants.TRAIN);
        mainPlane = planes[0];
        lastPlane = planes[planes.Count - 1];
        Debug.Log(lastPlane.name);
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
                    Debug.Log(plane.name);
                    Vector3 newPlanePosition = plane.transform.position;
                    Debug.Log(lastPlane.transform.position.x + (planeRadius * -2));
                    mainPlane.transform.position = new Vector3(lastPlane.transform.position.x + (planeRadius * -2), 0, 0);
                    lastPlane = mainPlane;
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
