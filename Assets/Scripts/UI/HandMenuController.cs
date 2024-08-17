using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMenuController : MonoBehaviour
{
    public float upThreshold = 0.8f; // Schwellenwert für die Ausrichtung nach oben (Dot-Produkt)

    private Transform[] childTransforms; // Alle direkten Kinder des Canvas

    void Start()
    {
        // Alle direkten Kinder des Canvas finden
        childTransforms = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childTransforms[i] = transform.GetChild(i);
        }
    }

    void Update()
    {
        // Überprüfen, ob das Menü nach oben zeigt
        bool isFacingUp = Vector3.Dot(transform.up, Vector3.up) > upThreshold;

        // Sichtbarkeit basierend auf der Ausrichtung setzen
        SetMenuVisibility(isFacingUp);
    }

    void SetMenuVisibility(bool isVisible)
    {
        // Alle direkten Kinder basierend auf der Sichtbarkeit ein-/ausschalten
        foreach (Transform child in childTransforms)
        {
            child.gameObject.SetActive(isVisible);
        }
    }
}