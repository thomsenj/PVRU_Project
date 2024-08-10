using UnityEngine;
using Dreamteck.Splines;
using System;

public class TrainFollower : MonoBehaviour
{
    public SplineFollower engineFollower; 
    public GameObject[] wagonFollowers; 
    public float distanceBetweenWagons = 10.0f;
    private SplineFollower[] splineFollowers;

    private void Start()
    {
        splineFollowers = new SplineFollower[wagonFollowers.Length];
        for (int i = 0; i < wagonFollowers.Length; i++)
        {
            SplineFollower follower = wagonFollowers[i].GetComponent<SplineFollower>();
            splineFollowers[i] = follower;
        }
    }

    void Update()
    {
        for (int i = 0; i < wagonFollowers.Length; i++)
        {
            // I dont know maybe we have to add the actual wagon length per different wagon
            double targetPercent = engineFollower.result.percent - ((i + 1) * CalculateIndividualDistance(wagonFollowers[i])) / engineFollower.spline.CalculateLength();
            if (targetPercent < 0) targetPercent += 1.0;
            splineFollowers[i].SetPercent(targetPercent);
        }
    }

    public float CalculateIndividualDistance(GameObject gameObject)
    {

        switch (gameObject.name)
        {
            case GeneralConstants.COAL_CARRIAGE:
                return 10.0f;
            case GeneralConstants.TRAIN_CARRIAGE:
                return 11.0f;
            case GeneralConstants.TRAIN_CARRIAGE + " 1":
                return 13.0f;
            case GeneralConstants.FREIGHT_CARRIAGE:
                return 12.0f;
            default:
                throw new UnexpectedNameException($"Unexpected GameObject name: {gameObject.name}");
        }
    }
    public class UnexpectedNameException : Exception
    {
        public UnexpectedNameException() : base("Unexpected GameObject name encountered.")
        {
        }

        public UnexpectedNameException(string message) : base(message)
        {
        }

        public UnexpectedNameException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
