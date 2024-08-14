using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PickaxeSwing : MonoBehaviour
{
    public XRGrabInteractable interactable;
    public float swingThreshold = 1.0f; 
    public bool isSwinging = false;

    private Vector3 lastPosition;
    private bool isPickedUp = false;


    public bool getIsSwinging() {
        return isSwinging;
    }

    void Start()
    {
        if (interactable == null){
            interactable = GetComponent<XRGrabInteractable>();
        }
        interactable.selectEntered.AddListener(OnGrabbed);
        interactable.selectExited.AddListener(OnReleased);
    }

    void Update()
    {
        if (isPickedUp)
        {
            Vector3 currentPosition = transform.position;
            float speed = (currentPosition - lastPosition).magnitude / Time.deltaTime;
            isSwinging = speed > swingThreshold;
            lastPosition = currentPosition;
        }
        else
        {
            isSwinging = false;
        }
    }

    void OnGrabbed(SelectEnterEventArgs args)
    {
        isPickedUp = true;
        lastPosition = transform.position;
    }

    void OnReleased(SelectExitEventArgs args)
    {
        isPickedUp = false;
        isSwinging = false;
    }
}
