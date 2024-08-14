using UnityEngine;
using System.IO.Ports;
using System.Threading.Tasks;

public class EMSAdapter : MonoBehaviour
{
    public string portName;

    private SerialPort serialPort;
    private bool isChannel1Active = false;
    private bool isChannel2Active = false;

    void Start()
    {
        // Set the name of the serial port and baud rate 
        // --> Windows > Device Manager > Ports (COM & LPT)
        // --> MacOS > ls /dev/tty.*
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
        SendEMSSignal("1");
    }

    public void ToggleChannel2()
    {
        SendEMSSignal("2");
    }

    public void TurnOffChannel1()
    {
      if (isChannel1Active)
      {
          SendEMSSignal("1");
      }
    }

    public void TurnOffChannel2()
    {
      if (isChannel2Active)
      {
          SendEMSSignal("2");
      }
    }

    public void turnOnChannel1()
    {
        if (!isChannel1Active)
        {
            SendEMSSignal("1");
        }
    }

    public void turnOnChannel2()
    {
        if (!isChannel2Active)
        {
            SendEMSSignal("2");
        }
    }

    public async Task sendImpulseChannel1(int impulseDurationMs)
    {
        turnOnChannel1();
        await Task.Delay(impulseDurationMs);
        TurnOffChannel1();
    }

    public async Task sendImpulseChannel2(int impulseDurationMs)
    {
        turnOnChannel2();
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
}