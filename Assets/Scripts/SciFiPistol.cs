using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SciFiPistol : MonoBehaviour
{
    public ParticleSystem particles;

    // Raycasts Components
    public LayerMask layerMask;
    public Transform shootSource;
    public float distance = 10; // Maximum distance of the Raycast
    private bool rayActivate = false;


    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(x => StartShoot());
        grabInteractable.deactivated.AddListener(x => StopShoot());
    }

    // Update is called once per frame
    void Update()
    {
        if (rayActivate) // Only Activate Ray when Shooting
        {
            RaycastCheck();
        }

    }

    public void StartShoot()
    {
        particles.Play();
        rayActivate = true;
    }

    public void StopShoot()
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        rayActivate = false;
    }

    void RaycastCheck()
    {
        RaycastHit hit; // Raycasts to detect when object was hit
        bool hasHit = Physics.Raycast(shootSource.position, shootSource.forward, out hit, distance, layerMask);

        if (hasHit)
        {
            hit.transform.gameObject.SendMessage("BreakItem", SendMessageOptions.DontRequireReceiver); // has to be the same name as the fuction its sending to!
        }
    }
}
