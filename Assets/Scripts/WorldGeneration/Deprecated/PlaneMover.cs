using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMover : MonoBehaviour
{
    public Direction direction;
    public float speed = 2f;
    
    private GameObject planeConfigurator;
    void Update()
    {
        try
        {
            planeConfigurator = GameObject.Find(GeneralConstants.PLANE_CONFIGURATOR_NAME);
            PlaneMoverConfigurator planeMoverConfigurator = planeConfigurator.GetComponent<PlaneMoverConfigurator>();
            speed = planeMoverConfigurator.getSpeed();
        } catch
        {
            Debug.Log("No PlaneMoverConfigurator in Scene. Constant speed.");
        }

        transform.position +=
            new Vector3(
                direction.Equals(Direction.X) ? speed : 0,
                direction.Equals(Direction.Y) ? speed : 0,
                direction.Equals(Direction.Z) ? speed : 0
            ) * Time.deltaTime;
    }
}

