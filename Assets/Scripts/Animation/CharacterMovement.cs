using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // variable to store character animator component
    private Animator animator;

    // variables to store the optimized setter/getter paramter IDs
    private int isWalkingHash;
    private int isRunningHash;

    // variable to store the instance of the PlayerInput
    PlayerInput input;

    // variables to store player input values
    [SerializeField] private Vector2 currentMovement;
    [SerializeField] private bool movementPressed;
    [SerializeField] private bool runPressed;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        input = new PlayerInput();

        // set the player input values using listeners
        input.CharacterControls.Movement.performed += ctx =>
        {
            currentMovement = ctx.ReadValue<Vector2>();
            movementPressed = currentMovement.x != 0 || currentMovement.y != 0;
        };
        input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        input.CharacterControls.Movement.canceled += ctx => movementPressed = false;
        // input.CharacterControls.Movement.performed += ctx => Debug.Log(ctx.ReadValueAsObject());
    }

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
        HandleMovement();
        HandleRotation();
    }

    void HandleRotation()
    {
        // Current position of our character
        Vector3 currentPosition = transform.position;

        // the change in position our character should point to
        Vector3 newPosition = new Vector3(currentMovement.x, 0, currentMovement.y);

        // combine the positions to give a position to look at
        Vector3 positionToLookAt = currentPosition + newPosition;

        // rotate the character to face the positionToLookAt
        transform.LookAt(positionToLookAt);
    }

    void HandleMovement()
    {
        // get paramter values from animator
        bool isRunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);

        // start walking if movementPressed is true and not already walking
        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        // stop walking if movementPressed is false and not already running
        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        // start running if movement pressed and run pressed is true and not already running
        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        // stop running if movement pressed or run pressed is false and currently running
        if ((!movementPressed || !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }

    }

    void OnEnable()
    {
        // enable the character controls action map
        input.CharacterControls.Enable();
    }

    void OnDisable()
    {
        // disable the character controls action map
        input.CharacterControls.Disable();
    }
}
