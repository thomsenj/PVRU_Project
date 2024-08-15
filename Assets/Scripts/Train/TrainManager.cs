using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class TrainManager : MonoBehaviour
{
    public WeatherTypes weatherType = WeatherTypes.DEFAULT;
    //private GameObject train;
    //private SplineFollower splineFollower;

    //private void Start()
    //{
    //    train = GameObject.FindGameObjectWithTag(TagConstants.TRAIN);
    //    splineFollower = train.GetComponent<SplineFollower>();
    //}

    //public void setSpeed(float speed)
    //{
        // maybe in the future
        //splineFollower.followSpeed = speed;
    //}

    public float getFuelModifier()
    {
        WeatherTypeInfo weatherTypeInfo = WeatherTypeExtensions.GetWeatherTypeInfo(weatherType);
        return weatherTypeInfo.FuelFactor;
    }

    public float getHeatModifier()
    {
        WeatherTypeInfo weatherTypeInfo = WeatherTypeExtensions.GetWeatherTypeInfo(weatherType);
        return weatherTypeInfo.HeatFactor;
    }
}
