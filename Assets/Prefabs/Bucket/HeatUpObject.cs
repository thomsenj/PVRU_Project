using UnityEngine;

public class HeatUpObject: MonoBehaviour
{
    public float temperature = 0f;      
    public float maxTemperature = 100f; 
    public float heatRate = 1f;         
    public float coolRate = 10f;
    public TrainManager trainManager;

    private float heatModifier = 1.0f;

    private void Start()
    {
        try
        {
            trainManager = GameObject.FindGameObjectWithTag(TagConstants.TRAIN_MANAGER).GetComponent<TrainManager>();
            heatModifier = trainManager.getHeatModifier();
        }
        catch
        {
            Debug.LogError("This scene lacks a train manager.");
        }
    }

    void Update()
    {
        if (temperature < maxTemperature)
        {
            temperature += (heatModifier * heatRate) * Time.deltaTime;
        }

        if (temperature > maxTemperature)
        {
            temperature = maxTemperature;
        }
        UpdateColor();
    }

    public void CoolDown(float amount)
    {

        temperature -= coolRate * amount;
        if (temperature < 0)
        {
            temperature = 0;
        }
        UpdateColor();
    }

    private void UpdateColor()
    {
        Color color = Color.Lerp(Color.blue, Color.red, temperature / maxTemperature);
        GetComponent<Renderer>().material.color = color;
    }
}
