using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class DreamTeckSplineGenerator : MonoBehaviour
{
    public int numberOfPoints = 10;
    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -50f;
    public float maxZ = 50f;

    public SplineComputer splineComputer;
    public GameObject splineFollowerGameObject;

    void Start()
    {
 
        //GenerateRandomSpline();

        GenerateSnakeSpline();
        splineFollowerGameObject.SetActive(true);

    }

    void GenerateRandomSpline()
    {
        SplinePoint[] points = new SplinePoint[numberOfPoints];

        for (int i = 0; i < numberOfPoints; i++)
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            Vector3 newPosition = new Vector3(randomX, 0, randomZ);

            points[i] = new SplinePoint(newPosition);
        }

        splineComputer.SetPoints(points);
    }

    void GenerateSnakeSpline()
    {
        SplinePoint[] points = new SplinePoint[numberOfPoints];
        float stepX = (maxX - minX) / (numberOfPoints - 1);
        float amplitudeZ = (maxZ - minZ) / 2f;
        float frequency = 2f * Mathf.PI / numberOfPoints;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float x = minX + i * stepX;
            float z = amplitudeZ * Mathf.Sin(frequency * i);
            Vector3 newPosition = new Vector3(x, 0, z);
            points[i] = new SplinePoint(newPosition);
        }

        splineComputer.SetPoints(points);
    }

}
