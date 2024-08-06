using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMoverConfigurator : MonoBehaviour
{
    public float speed =  2f;
    void Update()
    {

    }

    public void setSpeed(float speed)
    {
        this.speed = speed;
    }

    public float getSpeed()
    {
        return this.speed;
    }

}
