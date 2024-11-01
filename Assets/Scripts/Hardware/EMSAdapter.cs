using UnityEngine;
using System.IO.Ports;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System.Collections;

public class EMSAdapter : MonoBehaviour
{
    public string portName;
    public string microcontrollerIP = "192.168.1.100"; // Default IP address for the API
    public int microcontrollerPort = 80; // Default port for the API
    public bool useSerialCommunication = true; // Flag to determine if Serial or API should be used

    private SerialPort serialPort;
    private bool isChannel1Active = false;
    private bool isChannel2Active = false;

    void Start()
    {
        if (useSerialCommunication)
        {
            // Set up the serial port
            serialPort = new SerialPort(portName, 115200); // high baud rate for fast communication
            try
            {
                serialPort.Open();
                Debug.Log("Serial port opened successfully.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error opening serial port: " + ex.Message);
            }
        }
    }

    void OnApplicationQuit()
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }
    }

    public void ToggleChannel1()
    {
        if (useSerialCommunication)
        {
            SendEMSSignal("1");
        }
        else
        {
            StartCoroutine(SendEMSCommandViaAPI(1));
        }
    }

    public void ToggleChannel2()
    {
        if (useSerialCommunication)
        {
            SendEMSSignal("2");
        }
        else
        {
            StartCoroutine(SendEMSCommandViaAPI(2));
        }
    }

    public void TurnOffChannel1()
    {
        if (isChannel1Active)
        {
            ToggleChannel1();
        }
    }

    public void TurnOffChannel2()
    {
        if (isChannel2Active)
        {
            ToggleChannel2();
        }
    }

    public void TurnOnChannel1()
    {
        if (!isChannel1Active)
        {
            ToggleChannel1();
        }
    }

    public void TurnOnChannel2()
    {
        if (!isChannel2Active)
        {
            ToggleChannel2();
        }
    }

    public async Task SendImpulseChannel1(int impulseDurationMs)
    {
        TurnOnChannel1();
        await Task.Delay(impulseDurationMs);
        TurnOffChannel1();
    }

    public async Task SendImpulseChannel2(int impulseDurationMs)
    {
        TurnOnChannel2();
        await Task.Delay(impulseDurationMs);
        TurnOffChannel2();
    }

    private void SendEMSSignal(string command)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.WriteLine(command);
                Debug.Log("EMS signal sent: " + command);

                // Read the response from the EMS device
                string response = serialPort.ReadLine();
                Debug.Log("EMS response received: " + response);

                // Update the state based on the response
                UpdateChannelState(response);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Error sending EMS signal: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Serial port is not open.");
        }
    }

    private IEnumerator SendEMSCommandViaAPI(int channel)
    {
        string url = $"http://{microcontrollerIP}:{microcontrollerPort}/emsCommand?cmd={channel}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error sending EMS command via API: {request.error}");
            }
            else
            {
                Debug.Log($"EMS command sent via API: Toggle Channel {channel}");

                // After sending the command, sync the internal state with the server's status
                yield return StartCoroutine(SyncChannelState());
            }
        }
    }

    private IEnumerator SyncChannelState()
    {
        string url = $"http://{microcontrollerIP}:{microcontrollerPort}/status";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error syncing channel state via API: {request.error}");
            }
            else
            {
                string response = request.downloadHandler.text;
                Debug.Log("Status response received: " + response);

                // Update internal flags based on the status response
                isChannel1Active = response.Contains("\"channel1Active\":true");
                isChannel2Active = response.Contains("\"channel2Active\":true");
            }
        }
    }

    private void UpdateChannelState(string response)
    {
        if (response.Contains("Channel 1 active"))
        {
            isChannel1Active = true;
        }
        else if (response.Contains("Channel 1 inactive"))
        {
            isChannel1Active = false;
        }

        if (response.Contains("Channel 2 active"))
        {
            isChannel2Active = true;
        }
        else if (response.Contains("Channel 2 inactive"))
        {
            isChannel2Active = false;
        }
    }
}