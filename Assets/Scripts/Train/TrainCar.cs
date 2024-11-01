using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class TrainCar : MonoBehaviour
{
    [SerializeField] private SplineContainer trainTrack;
    // private Spline currentSpline;
    private Rigidbody rbTrainCar;
    [SerializeField] private TrainCar attachedTo;

    // Start is called before the first frame update
    void Start()
    {
        rbTrainCar = GetComponent<Rigidbody>();

        // currentSpline = trainTrack.Splines[0];
    }

    // FixedUpdate is used when doing movement and physics calculations
    // FixedUpdate is called before Update Calls
    void FixedUpdate()
    {
        // var native = new NativeSpline(currentSpline);
        var native = new NativeSpline(trainTrack.Spline);
        float distance = SplineUtility.GetNearestPoint(native, transform.position, out float3 nearest, out float tangent);

        transform.position = nearest;

        // Vector3 forward = Vector3.Normalize(native.EvaluateTangent(tangent));
        // Vector3 up = native.EvaluateUpVector(tangent);
        Vector3 forward = Vector3.Normalize(trainTrack.EvaluateTangent(tangent));
        Vector3 up = trainTrack.EvaluateUpVector(tangent);

        var remappedForward = new Vector3(0, 0, 1);
        var remappedUp = new Vector3(0, 1, 0);
        var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward, remappedUp));

        transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;

        Vector3 engineForward = transform.forward;

        if (Vector3.Dot(rbTrainCar.velocity, transform.forward) < 0)
        {
            engineForward *= -1;
        }

        rbTrainCar.velocity = rbTrainCar.velocity.magnitude * engineForward;
    }

    // private void HitJunction(Spline trainTrack)
    // {
    //     currentSpline = trainTrack;
    // }
}
