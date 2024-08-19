using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class TrainHealth : NetworkBehaviour
{
    [Networked]
    private int trainHealth { get; set; } = 40;
    public AudioSource damageSound;
    public HealthbarController controller;

    private void Start()
    {
        controller.SetMaxValue(41);
    }

    public void TakeDamage()
    {
        trainHealth--;
        if (damageSound != null)
        {
            damageSound.Play();
        }
        triggerEms();
        if (trainHealth == 0 || trainHealth < 0)
        {
            GameOverManager gameOverManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<GameOverManager>();
            gameOverManager.TriggerGameOver();
        }
        controller.SetValue(trainHealth);
    }

    private void triggerEms()
    {
        if (gameObject.GetComponent<EMSAdapter>() != null)
        {
            gameObject.GetComponent<EMSAdapter>().SendImpulseChannel1(500);
        }
    }
}
