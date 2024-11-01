using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ButtonOpensDoor : MonoBehaviour
{
    // public Animator animator;
    // public string boolName = "Open";

    [SerializeField] private bool doorOpen;
    [SerializeField] private List<GameObject> _doorsAndWindows = new List<GameObject>();
    [SerializeField] private TextMeshPro statusText;
    private XRSimpleInteractable interactable;

    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<XRSimpleInteractable>().selectEntered.AddListener(x => ToggleDoorOpen());
        interactable = GetComponent<XRSimpleInteractable>();

        if (interactable != null)
        {
            interactable.selectEntered.AddListener(x => ToggleDoorOpen());
        }
        doorOpen = false;
        UpdateStatusText();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ToggleDoorOpen()
    {
        // bool isOpen = animator.GetBool(boolName);
        // animator.SetBool(boolName, !isOpen);
        doorOpen = !doorOpen;

        foreach (var door in _doorsAndWindows)
        {
            door.SetActive(!doorOpen);
        }

        UpdateStatusText();
    }

    private void UpdateStatusText()
    {
        if (statusText != null)
        {
            string status = doorOpen ? "<color=green>Open</color>" : "<color=red>Closed</color>";
            statusText.text = $"Windows:\n{status}";
        }
    }
}
