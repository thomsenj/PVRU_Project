using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDimensionalStateController : MonoBehaviour
{
    // variable to store character animator component
    private Animator animator;
    [SerializeField] private float velocity = 0.0f;
    [SerializeField] private float acceleration = 0.1f;
    [SerializeField] private float deceleration = 0.5f;
    // variable to store the optimized setter/getter paramter IDs
    private int VelocityHash;

    // Start is called before the first frame update
    void Start()
    {
        // search the gameobject this script is attached to and get the animator component
        animator = GetComponent<Animator>();

        // set the ID reference (Hashes increase perfomance)
        VelocityHash = Animator.StringToHash("Velocity");
    }

    // Update is called once per frame
    void Update()
    {
        // input will be true if the player is pressing on the passed in key parameter
        // get key input from player
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        if (forwardPressed && velocity < 1.0f)
        {
            velocity += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocity > 0.0f)
        {
            velocity -= Time.deltaTime * deceleration;
        }

        if (!forwardPressed && velocity < 0.0f)
        {
            velocity = 0.0f;
        }
        // set the parameter to our local variable value
        animator.SetFloat(VelocityHash, velocity);
    }
}
