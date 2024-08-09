using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class CoalPileUIInteraction : MonoBehaviour
{
    private ActionBasedController rightHandController;
    public string rightHandControllerName = "RightHand Controller";
    public TMP_Text coalAmountText; // TextMeshPro Textfeld
    private CoalPile currentCoalPile;

    void Start()
    {
        GameObject rightHandControllerObject = GameObject.Find(rightHandControllerName);
        if (rightHandControllerObject != null)
        {
            rightHandController = rightHandControllerObject.GetComponent<ActionBasedController>();
            if (rightHandController == null)
            {
                Debug.LogError("XRController component not found on the RightHand Controller GameObject.");
            }
        }
        else
        {
            Debug.LogError("RightHand Controller GameObject not found in the scene.");
        }

        if (coalAmountText == null)
        {
            Debug.LogError("CoalAmountText reference is not set in the Inspector.");
        }
        else
        {
            coalAmountText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (rightHandController == null || coalAmountText == null)
        {
            return; // Exit if the controller or text is not set
        }

        RaycastHit hit;
        if (Physics.Raycast(rightHandController.transform.position, rightHandController.transform.forward, out hit, 5f))
        {
            CoalPile coalPile = hit.collider.GetComponent<CoalPile>();
            if (coalPile != null)
            {
                currentCoalPile = coalPile;
                UpdateUI(true);
            }
            else
            {
                UpdateUI(false);
            }
        }
        else
        {
            UpdateUI(false);
        }
    }

    void UpdateUI(bool show)
    {
        if (show && currentCoalPile != null)
        {
            coalAmountText.gameObject.SetActive(true);
            coalAmountText.text = "Coal Amount: " + currentCoalPile.coalAmount;
        }
        else
        {
            coalAmountText.gameObject.SetActive(false);
        }
    }
}
