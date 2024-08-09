using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class BurnCoal : MonoBehaviour
{
    public float fuelstand = 100f;
    public TrainManager trainManager;

    private float fuelModifier = 1.0f;

    private void Start()
    {
        try
        {
            trainManager = GameObject.FindGameObjectWithTag(TagConstants.TRAIN_MANAGER).GetComponent<TrainManager>();
            fuelModifier = trainManager.getFuelModifier();
        }
        catch
        {
            Debug.LogError("This scene lacks a train manager.");
        }
        StartCoroutine(BurnFuelOverTime());
    }

    void OnTriggerEnter(Collider other)
    {
        try
        {
            if (other.CompareTag(TagConstants.COAL))
            {
                addFuel();
                if (trainManager != null)
                {
                    trainManager.setSpeed(getSpeed());
                } else
                {
                    Debug.LogError("This scene lacks a train manager.");
                }
                Destroy(other.gameObject);
            }
        }
        catch
        {
            Debug.LogError("An error occurred during trigger handling.");
        }
    }

    private void addFuel()
    {
        fuelstand = Mathf.Clamp(fuelstand + 10, 0, 100); 
        Debug.Log(fuelstand);
    }

    private float getSpeed()
    {


        if (fuelstand >= 1 && fuelstand <= 10)
            return 1;
        else if (fuelstand > 10 && fuelstand <= 20)
            return 2;
        else if (fuelstand > 20 && fuelstand <= 30)
            return 3;
        else if (fuelstand > 30 && fuelstand <= 40)
            return 4;
        else if (fuelstand > 40 && fuelstand <= 50)
            return 5;
        else if (fuelstand > 50 && fuelstand <= 60)
            return 6;
        else if (fuelstand > 60 && fuelstand <= 70)
            return 7;
        else if (fuelstand > 70 && fuelstand <= 80)
            return 8;
        else if (fuelstand > 80 && fuelstand <= 90)
            return 9;
        else if (fuelstand > 90 && fuelstand <= 100)
            return 10;
        else
            return 0; 
    }

    private IEnumerator BurnFuelOverTime()
    {
        while (true)
        {
            fuelstand = Mathf.Clamp(fuelstand - (1 *  fuelModifier), 0, 100);
            try { trainManager.setSpeed(getSpeed()); }
            catch { Debug.LogError("Could not set train speed."); }
            yield return new WaitForSeconds(5f);
            Debug.Log(fuelstand);
        }
    }
}
