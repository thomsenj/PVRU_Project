using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class DisableHandOnGrab : MonoBehaviour
{
    public GameObject rightHandModel;
    public GameObject leftHandModel;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(HideGrabbingHand);
        grabInteractable.selectExited.AddListener(ShowGrabbingHand);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void HideGrabbingHand(SelectEnterEventArgs args)
    {
        if (args.interactableObject.transform.tag == "Right Hand")
        {
            rightHandModel.SetActive(false);
        }
        else if (args.interactableObject.transform.tag == "Left Hand")
        {
            leftHandModel.SetActive(false);
        }
    }

    public void ShowGrabbingHand(SelectExitEventArgs args)
    {
        if (args.interactableObject.transform.tag == "Right Hand")
        {
            rightHandModel.SetActive(true);
        }
        else if (args.interactableObject.transform.tag == "Left Hand")
        {
            leftHandModel.SetActive(true);
        }
    }
}
