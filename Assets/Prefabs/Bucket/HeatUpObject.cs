using UnityEngine;

public class HeatUpObject: MonoBehaviour
{
    public float temperature = 0f;      
    public float maxTemperature = 100f; 
    public float heatRate = 1f;         
    public float coolRate = 10f;
    private TrainManager trainManager;
    private  GameOverManager gameOverManager;

    private float heatModifier = 1.0f;

    private void Start()
    {
        try
        {
            trainManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<TrainManager>();
            gameOverManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<GameOverManager>();
            heatModifier = trainManager.getHeatModifier();
        }
        catch
        {
            //Debug.LogError("This scene lacks a train manager.");
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
        if(temperature == maxTemperature) {
            gameOverManager.TriggerGameOver();
        }
    }

    public void CoolDown(float amount)
    {

        temperature -= coolRate * amount;
        if (temperature < 0)
        {
            temperature = 0;
        }
    }
}
