using UnityEngine;

public class HeatUpObject: MonoBehaviour
{
    public float temperature = 0f;      
    public float maxTemperature = 100f; 
    public float heatRate = 1f;         
    public float coolRate = 10f;        

    void Update()
    {
        if (temperature < maxTemperature)
        {
            temperature += heatRate * Time.deltaTime;
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
