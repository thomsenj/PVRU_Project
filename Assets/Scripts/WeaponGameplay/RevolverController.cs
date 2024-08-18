using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;
using Fusion; 

public class RevolverController : NetworkBehaviour
{
    public Transform attachPoint;
    public float detachThreshold = 3.0f;
    private bool isFalling = false;

    public float resetDelay = 5.0f;


    public Transform muzzle;                    
    public float recoilAmount = 0.25f;          
    public float recoilSpeed = 25f;             
    public BulletController bulletPrefab;        
    public ParticleSystem shellEjectParticle;
    public AudioSource gunshotSound;

    public Transform cylinder;                  
    public Transform hammer;                    
    public Transform trigger;                   

    private bool canShoot = true;

    private Rigidbody rb;
    private Transform originalParent;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private bool CheckIfIsFalling()
    {
        if (attachPoint == null)
        {
            return false;
        }
        return Vector3.Distance(transform.position, attachPoint.position) > detachThreshold && transform.position.y < attachPoint.position.y;
    }

    public override void FixedUpdateNetwork()
    {
        if (!isFalling && CheckIfIsFalling())
        {
            isFalling = true;
            transform.SetParent(null);
            StartCoroutine(HandleFallingRevolver());
        }
        rb.isKinematic = IsOnAttachPoint();
    }

    private bool IsOnAttachPoint()
    {
        return Vector3.Distance(transform.position, attachPoint.position) < 0.1f;
    }

    private IEnumerator HandleFallingRevolver()
    {
        yield return new WaitForSeconds(resetDelay);
        if (attachPoint != null && CheckIfIsFalling())
        {
            ResetRevolverPosition();
        }
        isFalling = false;
    }

    private void ResetRevolverPosition()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = attachPoint.position;
        transform.rotation = attachPoint.rotation; 
        transform.SetParent(originalParent); 
    }

    public void Shoot()
    {
        if(canShoot && !IsOnAttachPoint())
        {
            canShoot = false;

            if (gunshotSound != null)
            {
                gunshotSound.Play();
            }

            BulletController bullet = gameObject.GetComponent<BulletPoolManager>().SpawnBullet(muzzle.position);
            bullet.Shoot(muzzle.position, muzzle.forward);

            if (shellEjectParticle != null)
            {
                shellEjectParticle.Play();
            }

            AnimateRevolver();
            // wait for the animation to finish before allowing the player to shoot again
            StartCoroutine(ResetCanShoot());
        }
    }

    private IEnumerator ResetCanShoot()
    {
        yield return new WaitForSeconds(0.5f);
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
