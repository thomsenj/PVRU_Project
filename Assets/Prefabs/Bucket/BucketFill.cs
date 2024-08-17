using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketFill : MonoBehaviour
{
    public WaterBucket waterBucket;

    private Vector3 initialScale; // Die anfängliche Skalierung des Zylinders
    private Vector3 initialPosition; // Die anfängliche Position des Zylinders
    private Renderer bucketRenderer; // Der Renderer des Zylinders

    private const float minVisiblePercentage = 0.05f;

    void Start()
    {
        initialScale = transform.localScale;
        initialPosition = transform.localPosition;
        bucketRenderer = GetComponent<Renderer>(); // Holen des Renderer-Komponenten
    }

    void Update()
    {
        // Füllstand als Prozentsatz (Wert zwischen 0 und 1)
        float fillPercentage = waterBucket.GetWaterPercentage();

        if (fillPercentage < minVisiblePercentage)
        {
            bucketRenderer.enabled = false; // Deaktiviert die Sichtbarkeit des Zylinders
        }
        else
        {
            bucketRenderer.enabled = true; // Aktiviert die Sichtbarkeit des Zylinders

            Vector3 newScale = initialScale;
            newScale.y = initialScale.y * fillPercentage;
            transform.localScale = newScale;

            Vector3 newPosition = initialPosition;
            newPosition.y = initialPosition.y - (initialScale.y - newScale.y) / 2;
            transform.localPosition = newPosition;
        }
    }
}