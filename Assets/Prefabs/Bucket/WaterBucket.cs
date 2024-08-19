using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Collections;

public class WaterBucket : NetworkBehaviour
{
    public ParticleSystem waterParticles;
    public Transform attachPoint;
    public CollectableBankController controller;

    public float maxWaterAmount = 100f;
    [Networked]
    public float currentWaterAmount { get; set; }
    public float pourRate = 10f;

    private Rigidbody rb;

    public AudioSource splashSound;

    public float detachThreshold = 2.0f;
    public float resetDelay = 3.0f; 
    private bool isFalling = false;
    private Transform originalParent;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentWaterAmount = maxWaterAmount;
        originalParent = transform.parent;
    }

    public override void FixedUpdateNetwork()
    {
        if (!isFalling && CheckIfIsFalling())
        {
            isFalling = true;
            transform.SetParent(null);
            StartCoroutine(HandleFallingBucket());
        }

        if (IsBucketTilted())
        {
            PourWater();
        }
        else
        {
            StopPouring();
        }
        controller.SetCount((int)(currentWaterAmount));
    }

    private bool CheckIfIsFalling()
    {
        if (attachPoint == null)
        {
            return false;
        }
        return Vector3.Distance(transform.position, attachPoint.position) > detachThreshold && transform.position.y < attachPoint.position.y;
    }

    public void FillUp()
    {
        currentWaterAmount += 50;
        if(currentWaterAmount > maxWaterAmount)
        {

            currentWaterAmount = maxWaterAmount;
        }
    }

    private bool IsBucketTilted()
    {
        return Vector3.Dot(transform.up, Vector3.up) < 0.65f;
    }

    public float GetWaterPercentage()
    {
        return currentWaterAmount / maxWaterAmount;
    }

    private void PourWater()
    {
        Debug.Log(currentWaterAmount);
        if (currentWaterAmount > 0)
        {
            if (!waterParticles.isPlaying)
            {
                waterParticles.Play();
            }

            if (splashSound != null)
            {
                splashSound.Play();
            }


            currentWaterAmount -= pourRate * Time.deltaTime;
            if (currentWaterAmount <= 0)
            {
                currentWaterAmount = 0;
                StopPouring();
            }
        }
    }

    private void StopPouring()
    {
        if (waterParticles.isPlaying)
        {
            waterParticles.Stop();
        }
    }

    private IEnumerator HandleFallingBucket()
    {
        yield return new WaitForSeconds(resetDelay);

        if (attachPoint != null && CheckIfIsFalling())
        {
            ResetBucketPosition();
        }

        isFalling = false;
    }

    private void ResetBucketPosition()
    {
        rb.velocity = Vector3.zero; // Stoppt die Bewegung des Eimers
        rb.angularVelocity = Vector3.zero; // Stoppt die Rotation des Eimers
        transform.SetPositionAndRotation(attachPoint.position, attachPoint.rotation);
        transform.SetParent(attachPoint.parent);
    }
}