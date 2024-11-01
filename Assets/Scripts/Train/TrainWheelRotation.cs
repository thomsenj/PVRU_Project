using System.Collections.Generic;
using UnityEngine;

public class TrainWheelRotator : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _BigTrainWheels;

    [SerializeField]
    private List<GameObject> _SmallTrainWheels;

    [SerializeField]
    private float turnSpeedBig = 1f;

    [SerializeField]
    private float speedMultiplier = 2f;

    void Update()
    {
        RotateWheels();
    }

    private void RotateWheels()
    {
        foreach (var wheel in _BigTrainWheels)
        {
            if (wheel != null)
            {
                wheel.transform.Rotate(Vector3.right * turnSpeedBig * Time.deltaTime);
            }
        }
        float turnSpeedSmall = turnSpeedBig * speedMultiplier;
        foreach (var wheel in _SmallTrainWheels)
        {
            if (wheel != null)
            {
                wheel.transform.Rotate(Vector3.right * turnSpeedSmall * Time.deltaTime);
            }
        }
    }
}
