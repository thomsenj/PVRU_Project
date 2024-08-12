using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlanePool : MonoBehaviour
{

    public List<GameObject> planes;

    private System.Random random = new System.Random();

    public void AddPlane(GameObject gameObject)
    {
        planes.Add(gameObject);
    }

    public GameObject GetAndRemoveRandomGameObject()
    {
        if (planes.Count == 0)
        {
            return null; 
        }

        int randomIndex = random.Next(planes.Count);
        GameObject randomGameObject = planes[randomIndex];
        planes.RemoveAt(randomIndex);
        return randomGameObject;
    }
}