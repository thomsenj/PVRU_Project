using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class TrainHealth : NetworkBehaviour
{
    [Networked]
    private int trainHealth { get; set; } = 3;
    public AudioSource damageSound;

    public void TakeDamage()
    {
        Debug.Log("Method works");
        trainHealth--;
        if (damageSound != null)
        {
            damageSound.Play();
        }
        triggerEms();
        if (trainHealth == 0 || trainHealth < 0) { 
            GameOverManager gameOverManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<GameOverManager>();
            gameOverManager.TriggerGameOver();
        }
    }

    private void triggerEms()
    {
        if (gameObject.GetComponent<EMSAdapter>() != null)
        {
            gameObject.GetComponent<EMSAdapter>().SendImpulseChannel1(500);
        }
    }
}
