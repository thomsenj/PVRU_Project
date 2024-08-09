using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateControllerBoolean : MonoBehaviour
{
    // variable to store character animator component
    private Animator animator;
    // variables to store the optimized setter/getter paramter IDs
    private int isWalkingHash;
    private int isRunningHash;

    // Start is called before the first frame update
    void Start()
    {
        // search the gameobject this script is attached to and get the animator component
        animator = GetComponent<Animator>();
        // set the ID references (Hashes increase perfomance)
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);
        // input will be true if the player is pressing on the passed in key parameter
        // get key input from player
        bool forwardPressed = Input.GetKey(KeyCode.W);
        bool runPressed = Input.GetKey(KeyCode.LeftShift);

        // if player presses w key
        if (!isWalking && forwardPressed)
        {
            // then set the isWalking boolean to be true
            animator.SetBool(isWalkingHash, true);
        }

        // if player is not prssing w key
        if (isWalking && !forwardPressed)
        {
            // then set the isWalking boolean to be false
            animator.SetBool(isWalkingHash, false);
        }

        // if player is walking and not running and presses left shift
        if (!isRunning && (forwardPressed && runPressed))
        {
            // then set the isRunning boolean to be true
            animator.SetBool(isRunningHash, true);
        }

        // if player is running and stops running or stops walking
        if (isRunning && (!forwardPressed || !runPressed))
        {
            // then set the isRunning boolean to be false
            animator.SetBool(isRunningHash, false);
        }
    }
}
