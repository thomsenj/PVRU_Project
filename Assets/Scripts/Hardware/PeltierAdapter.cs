using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PeltierAdapter : MonoBehaviour
{
    public string microcontrollerIP = "192.168.1.100"; // Default IP address
    public int microcontrollerPort = 80; // Default port

    private float lastTemperatureLimit = float.MinValue;
    private float lastAimTemperature = float.MinValue;
    private float lastPercentage = float.MinValue;

    // Start is called before the first frame update
    void Start()
    {
        // Example usage
        StartCoroutine(GetTemperature());
    }

    // Method to set the IP address and port of the microcontroller
    public void SetMicrocontrollerAddress(string ip, int port)
    {
        microcontrollerIP = ip;
        microcontrollerPort = port;
    }

    // Coroutine to get the current temperature
    IEnumerator GetTemperature()
    {
        string url = $"http://{microcontrollerIP}:{microcontrollerPort}/temperature";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error getting temperature: {request.error}");
            }
            else
            {
                Debug.Log($"Current Temperature: {request.downloadHandler.text}");
            }
        }
    }

    // Coroutine to set the Peltier element state (on/off)
    IEnumerator SetPeltierState(bool state)
    {
        string url = state ? $"http://{microcontrollerIP}:{microcontrollerPort}/peltier/on" : $"http://{microcontrollerIP}:{microcontrollerPort}/peltier/off";
        Debug.Log(url);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error setting Peltier state: {request.error}");
            }
            else
            {
                Debug.Log($"Peltier state set to: {(state ? "ON" : "OFF")}");
            }
        }
    }

    // Coroutine to set the temperature limit
    IEnumerator SetTemperatureLimit(float limit)
    {
        string url = $"http://{microcontrollerIP}:{microcontrollerPort}/setLimit?limit={limit}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error setting temperature limit: {request.error}");
            }
            else
            {
                Debug.Log($"Temperature limit set to: {limit}°C");
            }
        }
    }

    // Coroutine to set the aim temperature
    IEnumerator SetAimTemperature(float temperature)
    {
        string url = $"http://{microcontrollerIP}:{microcontrollerPort}/setTemperature?temperature={temperature}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error setting aim temperature: {request.error}");
            }
            else
            {
                Debug.Log($"Aim temperature set to: {temperature}°C");
            }
        }
    }

    // Coroutine to set the percentage
    IEnumerator SetPercentage(float percentage)
    {
        string url = $"http://{microcontrollerIP}:{microcontrollerPort}/setPercentage?percentage={percentage*100}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error setting percentage: {request.error}");
            }
            else
            {
                Debug.Log($"Percentage set to: {percentage}%");
            }
        }
    }

    // Lazy method to set the Peltier state
    public void LazySetPeltierState(bool state)
    {
        StartCoroutine(SetPeltierState(state));
    }

    // Lazy method to get the temperature
    public void LazyGetTemperature()
    {
        StartCoroutine(GetTemperature());
    }

    // Lazy method to set the temperature limit with threshold
    public void LazySetTemperatureLimit(float limit, float threshold)
    {
        if (Mathf.Abs(limit - lastTemperatureLimit) > threshold)
        {
            lastTemperatureLimit = limit;
            StartCoroutine(SetTemperatureLimit(limit));
        }
    }

    // Lazy method to set the aim temperature with threshold
    public void LazySetAimTemperature(float temperature, float threshold)
    {
        if (Mathf.Abs(temperature - lastAimTemperature) > threshold)
        {
            lastAimTemperature = temperature;
            StartCoroutine(SetAimTemperature(temperature));
        }
    }

    // Lazy method to set the percentage with threshold
    public void LazySetPercentage(float percentage, float threshold)
    {
        if (Mathf.Abs(percentage - lastPercentage) > threshold)
        {
            lastPercentage = percentage;
            StartCoroutine(SetPercentage(percentage));
        }
    }
}