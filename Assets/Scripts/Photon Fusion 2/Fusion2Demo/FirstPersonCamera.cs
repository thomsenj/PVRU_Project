using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform target;
    public float mouseSensitivity = 10.0f;

    private float verticalRotation;
    private float horizontalRotation;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = target.position;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

        horizontalRotation += mouseX * mouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }
}