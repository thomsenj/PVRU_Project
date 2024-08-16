using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class RevolverController : MonoBehaviour
{
    public Transform muzzle;                    
    public float recoilAmount = 0.25f;          
    public float recoilSpeed = 25f;             
    public BulletController bulletPool;        
    public ParticleSystem shellEjectParticle;   

    public Transform cylinder;                  
    public Transform hammer;                    
    public Transform trigger;                   

    private bool isRecoiling = false;           
    private Vector3 recoilPosition;             
    private Quaternion recoilRotation;          
    private bool canShoot = true;               

    public void Shoot()
    {
        canShoot = false;

        bulletPool.Shoot(muzzle.position, muzzle.forward);

        if (shellEjectParticle != null)
        {
            shellEjectParticle.Play();
        }

        AnimateRevolver();
        canShoot = true;
    }

    private void StartRecoil()
    {
        isRecoiling = true;

        recoilPosition = transform.localPosition + new Vector3(0, 0, -recoilAmount * 0.5f);
        recoilRotation = transform.localRotation * Quaternion.Euler(-recoilAmount * 100f, 0, 0);
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
