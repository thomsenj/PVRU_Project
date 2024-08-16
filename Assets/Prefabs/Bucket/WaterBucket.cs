using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class WaterBucket : MonoBehaviour
{
    public ParticleSystem waterParticles; 
    private bool isPouring = false;

    public float maxWaterAmount = 100f; 
    private float currentWaterAmount;   
    public float pourRate = 10f;        

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    public HeatUpObject heatUpObject; 

    private Vector3 startPos;

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(StartPouring);
        grabInteractable.selectExited.AddListener(StopPouring);

        currentWaterAmount = maxWaterAmount;
        startPos = gameObject.transform.position;
    }

    void Update()
    {
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

    private void StartPouring(SelectEnterEventArgs args)
    {
        if (currentWaterAmount > 0)
        {
            isPouring = true;
        }
    }

    private void StopPouring(SelectExitEventArgs args)
    {
        isPouring = false;
        waterParticles.Stop();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ( collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Ground"))
        {
            gameObject.transform.position = startPos;
            isPouring = false;
        }
    }
}
