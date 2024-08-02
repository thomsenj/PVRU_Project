using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonOpensDoor : MonoBehaviour
{
    // public Animator animator;
    // public string boolName = "Open";

    [SerializeField] private bool doorOpen;
    public GameObject door1;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => ToggleDoorOpen());
        doorOpen = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleDoorOpen()
    {
        // bool isOpen = animator.GetBool(boolName);
        // animator.SetBool(boolName, !isOpen);
        if (!doorOpen)
        {
            door1.SetActive(false);
            doorOpen = true;
        }
        else if (doorOpen)
        {
            door1.SetActive(true);
            doorOpen = false;
        }
    }
}
