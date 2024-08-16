using UnityEngine;
using UnityEngine.XR.Hands.Gestures;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;
using UnityEngine.XR.Hands.Samples.GestureSample;

public class WeaponControllerGesture : MonoBehaviour
{
    public Transform muzzle;
    public float recoilAmount = 0.25f;
    public float recoilSpeed = 25f;
    public BulletPoolManager bulletPool;
    public ParticleSystem shellEjectParticle;

    public Transform cylinder;
    public Transform hammer;
    public Transform trigger;

    private bool canShoot = true;
    private XRGrabInteractable grabInteractable;
    private int fireCount = 0; // Counter to track the number of shots fired

    [SerializeField]
    [Tooltip("The hand shape or pose that must be detected for the gesture to be performed.")]
    private ScriptableObject m_HandShapeOrPose; // Reference to the ScriptableObject defining the gesture

    private XRHandShape m_HandShape;
    private XRHandPose m_HandPose;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrabbed);
        grabInteractable.selectExited.AddListener(OnReleased);

        // Cast the ScriptableObject to its specific type (HandShape or HandPose)
        m_HandShape = m_HandShapeOrPose as XRHandShape;
        m_HandPose = m_HandShapeOrPose as XRHandPose;

        // If it's a pose, set the target transform (optional, based on your setup)
        if (m_HandPose != null && m_HandPose.relativeOrientation != null)
        {
            m_HandPose.relativeOrientation.targetTransform = transform;
        }

        if (m_HandShape != null || m_HandPose != null)
        {
            // Add listeners to detect when the gesture is performed
            var gestureSample = GetComponent<StaticHandGesture>();
            gestureSample.gesturePerformed.AddListener(OnShootGesturePerformed);
            gestureSample.gestureEnded.AddListener(OnShootGestureEnded);
        }
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        canShoot = true;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        canShoot = false;
    }

    private void OnShootGesturePerformed()
    {
        if (canShoot)
        {
            Shoot();
        }
    }

    private void OnShootGestureEnded()
    {
        // Handle any cleanup if necessary when the gesture ends
    }

    public void Shoot()
    {
        canShoot = false;

        BulletController bullet = bulletPool.GetBullet();
        bullet.Shoot(muzzle.position, muzzle.forward);

        // Log the event of firing the weapon
        fireCount++;
        Debug.Log($"Weapon fired! Total shots fired: {fireCount}");

        if (shellEjectParticle != null)
        {
            shellEjectParticle.Play();
        }

        AnimateRevolver();
        canShoot = true;
    }

    private void AnimateRevolver()
    {
        StartCoroutine(AnimateCylinder());
        StartCoroutine(AnimateHammer());
        StartCoroutine(AnimateTrigger());
    }

    private IEnumerator AnimateCylinder()
    {
        float animationTime = 0.25f;
        float elapsedTime = 0f;
        Quaternion initialRotation = cylinder.localRotation;
        Quaternion finalRotation = initialRotation * Quaternion.Euler(0, 0, 60f);

        while (elapsedTime < animationTime)
        {
            cylinder.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cylinder.localRotation = finalRotation;
    }

    private IEnumerator AnimateHammer()
    {
        float animationTime = 0.2f;
        float elapsedTime = 0f;
        Quaternion initialRotation = hammer.localRotation;
        Quaternion finalRotation = Quaternion.Euler(-45f, 0, 0);

        while (elapsedTime < animationTime)
        {
            hammer.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < animationTime)
        {
            hammer.localRotation = Quaternion.Slerp(finalRotation, initialRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hammer.localRotation = initialRotation;
    }

    private IEnumerator AnimateTrigger()
    {
        float animationTime = 0.2f;
        float elapsedTime = 0f;
        Quaternion initialRotation = trigger.localRotation;
        Quaternion finalRotation = Quaternion.Euler(35f, 0, 0);

        while (elapsedTime < animationTime)
        {
            trigger.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < animationTime)
        {
            trigger.localRotation = Quaternion.Slerp(finalRotation, initialRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trigger.localRotation = initialRotation;
    }
}
