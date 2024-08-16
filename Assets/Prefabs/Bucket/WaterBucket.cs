using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WaterBucket : MonoBehaviour
{
    public ParticleSystem waterParticles;
    private bool isPouring = false;

    public float maxWaterAmount = 100f;
    private float currentWaterAmount;
    public float pourRate = 10f;

    private Rigidbody rb;

    public HeatUpObject heatUpObject;

    private Vector3 startPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentWaterAmount = maxWaterAmount;
        startPos = transform.localPosition;
    }

    void Update()
    {
        if(!isPouring && transform.position != startPos){
            isPouring = true;
        }
        if (isPouring && IsBucketTilted())
        {
            PourWater();
        }
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


            //currentWaterAmount -= pourRate * Time.deltaTime;
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
