using UnityEngine;

public class HeatUpObject : MonoBehaviour
{
    public float temperature = 0f;
    public float maxTemperature = 100f;
    public float heatRate = 1f;
    public float coolRate = 10f;

    private TrainManager trainManager;
    private GameOverManager gameOverManager;
    private float heatModifier = 1.0f;

    public bool showHeatColor = true; // Parameter für Farbwechsel
    private Renderer objectRenderer; // Referenz zum Renderer des Objekts
    private PeltierAdapter adapter;

    private void Start()
    {
        adapter = gameObject.GetComponent<PeltierAdapter>();
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

        objectRenderer = GetComponent<Renderer>(); // Renderer-Referenz abrufen
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

        if (temperature == maxTemperature)
        {
            gameOverManager.TriggerGameOver();
        }

        if (showHeatColor && objectRenderer != null)
        {
            UpdateHeatColor();
        }
        adapter.LazySetPercentage(GetPercentage(), 0.05f);
    }

    private float GetPercentage()
    {
        return temperature / maxTemperature;
    }

    public void CoolDown(float amount)
    {
        Debug.Log("Cool down");
        temperature -= coolRate * amount;
        if (temperature < 0)
        {
            temperature = 0;
        }
    }

    private void UpdateHeatColor()
    {
        float colorValue = temperature / maxTemperature;
        Color heatColor = Color.Lerp(Color.blue, Color.red, colorValue);
        objectRenderer.material.color = heatColor;
    }

    // Diese Methode erkennt Kollisionen von Partikeln mit dem Objekt
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("Collision");
        if (other.CompareTag("WaterParticle"))
        {
            CoolDown(0.01f); 
        }
    }
}