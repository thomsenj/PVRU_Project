using UnityEngine;
using System.IO.Ports;


public class EMSAdapter : MonoBehaviour
{
    public string portName;

    private SerialPort serialPort;

    void Start()
    {
        // Set the name of the serial port and baud rate --> Windows > Device Manager > Ports (COM & LPT)
        serialPort = new SerialPort(portName, 9600);
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

    void OnMouseDown()
    {
        // Change Channel: C2 
        SendEMSSignal("GC1T9000I50");
    }

    private void SendEMSSignal(string command)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            try
            {
                serialPort.WriteLine(command);
                Debug.Log("EMS signal sent: " + command);
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
