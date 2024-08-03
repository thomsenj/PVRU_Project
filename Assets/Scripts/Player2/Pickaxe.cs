using System.Collections;
using UnityEngine;

public class Pickaxe : MonoBehaviour
{
    public GameObject heldPickaxe; 
    public float swingDuration = 0.3f; 
    private bool isActive = false;
    private bool isSwinging = false;

    void Update()
    {
        // Toggle Pickaxe Visibility with P key
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Keydown P works");
            isActive = !isActive;
            heldPickaxe.SetActive(isActive);
        }

        // Swing Pickaxe with Left Mouse Button
        if (Input.GetMouseButtonDown(0) && isActive && !isSwinging)
        {
            StartCoroutine(SwingPickaxe());
        }
    }

    private IEnumerator SwingPickaxe()
    {
        isSwinging = true;

        Quaternion initialRotation = heldPickaxe.transform.localRotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, -90); 

        float elapsedTime = 0;
        float halfSwingDuration = swingDuration / 4;

        while (elapsedTime < halfSwingDuration)
        {
            heldPickaxe.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / halfSwingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        RaycastHit hit;
        if (Physics.Raycast(heldPickaxe.transform.position, heldPickaxe.transform.forward, out hit, 2.0f))
        {
            Debug.Log($"Hit detected on: {hit.collider.name}");
            Resource resource = hit.collider.GetComponent<Resource>();
            if (resource != null)
            {
                resource.Harvest();
            }
        }

        yield return new WaitForSeconds(0.05f);

        elapsedTime = 0;
        while (elapsedTime < halfSwingDuration)
        {
            heldPickaxe.transform.localRotation = Quaternion.Slerp(targetRotation, initialRotation, elapsedTime / halfSwingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        heldPickaxe.transform.localRotation = initialRotation;
        isSwinging = false;
    }
}
