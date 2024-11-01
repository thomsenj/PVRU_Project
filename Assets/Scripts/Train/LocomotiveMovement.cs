using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocomotiveMovement : MonoBehaviour
{
    private Rigidbody rb;
    // [SerializeField] private float power;
    [SerializeField] private float accelerationRate = 2f;
    [SerializeField] private float maxSpeed = 50f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // FixedUpdate is used when doing movement and physics calculations
    // FixedUpdate is called before Update Calls
    void FixedUpdate()
    {
        TrainThrottle(accelerationRate);
    }

    private void TrainThrottle(float force)
    {
        if (rb.velocity.magnitude < maxSpeed)
        {
            Vector3 moveDirection = force * transform.forward;
            rb.AddForce(moveDirection);
        }
    }
}
