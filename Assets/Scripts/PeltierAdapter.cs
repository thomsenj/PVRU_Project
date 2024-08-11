using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PeltierAdapter : MonoBehaviour
{
    public string microcontrollerIP = "192.168.1.100"; // Default IP address
    public int microcontrollerPort = 80; // Default port

    // Start is called before the first frame update
    void Start()
    {
        // Example usage
        StartCoroutine(GetTemperature());
        LazySetPeltierState(true);
        StartCoroutine(SetTemperatureLimit(30.0f));
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

    // Lazy method to set the temperature limit
    public void LazySetTemperatureLimit(float limit)
    {
        StartCoroutine(SetTemperatureLimit(limit));
    }

    // Lazy method to set the aim temperature
    public void LazySetAimTemperature(float temperature)
    {
        StartCoroutine(SetAimTemperature(temperature));
    }
}