using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager == null)
        {
            Debug.LogError("No GameOverManager found in the scene.");
            return;
        }
        if (other.CompareTag(TagConstants.GAME_OVER_COLLIDER))
        {
            Debug.Log("Triggered with Player2, triggering Game Over.");
            gameOverManager.TriggerGameOver();
        }
    }
}
