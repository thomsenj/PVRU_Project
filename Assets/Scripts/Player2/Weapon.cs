using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject heldWeapon;
    public float swingDuration = 0.3f;
    public int baseDamage = 25;
    private bool isActive = false;
    private bool isSwinging = false;

    void Update()
    {
        // Toggle Weapon Visibility with 2 key
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isActive = !isActive;
            heldWeapon.SetActive(isActive);
        }

        // Swing Pickaxe with Left Mouse Button
        if (Input.GetMouseButtonDown(0) && isActive && !isSwinging)
        {
            StartCoroutine(SwingWeapon());
        }
    }
    
    public int getDamage() { return baseDamage; }

    public bool IsSwinging()
    {
        return isSwinging;
    }

    public void setSwinging(bool isSwinging)
    {
        this.isSwinging = isSwinging;
    }

    private IEnumerator SwingWeapon()
    {
        isSwinging = true;

        Quaternion initialRotation = heldWeapon.transform.localRotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, 0, -90);

        float elapsedTime = 0;
        float halfSwingDuration = swingDuration / 4;

        while (elapsedTime < halfSwingDuration)
        {
            heldWeapon.transform.localRotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / halfSwingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(0.05f);

        elapsedTime = 0;
        while (elapsedTime < halfSwingDuration)
        {
            heldWeapon.transform.localRotation = Quaternion.Slerp(targetRotation, initialRotation, elapsedTime / halfSwingDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        heldWeapon.transform.localRotation = initialRotation;
        isSwinging = false;
    }
}
