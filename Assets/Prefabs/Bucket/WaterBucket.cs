using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WaterBucket : NetworkBehaviour
{
    public ParticleSystem waterParticles;
    private bool isPouring = false;

    public float maxWaterAmount = 100f;
    [Networked]
    public float currentWaterAmount { get; set; }
    public float pourRate = 10f;

    private Rigidbody rb;

    public HeatUpObject heatUpObject;
    public AudioSource splashSound;

    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentWaterAmount = maxWaterAmount;
        startPos = transform.localPosition;
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (Object.HasStateAuthority)
        {
            if (!isPouring && transform.position != startPos)
            {
                isPouring = true;
            }
            if (isPouring && IsBucketTilted())
            {
                PourWater();
            }
        }
    }

    public void FillUp()
    {
        currentWaterAmount = currentWaterAmount + 50; 
    }

    private bool IsBucketTilted()
    {

        return Vector3.Dot(transform.up, Vector3.up) < 0.5f;
    }

    private void PourWater()
    {
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
                waterParticles.Stop();
                isPouring = false;
            }


            if (heatUpObject != null)
            {
                heatUpObject.CoolDown(pourRate * Time.deltaTime);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Ground"))
        {
            ResetBucketPosition();
            isPouring = false;
        }
    }

    private void ResetBucketPosition()
    {
        transform.localPosition = startPos;
    }
}
