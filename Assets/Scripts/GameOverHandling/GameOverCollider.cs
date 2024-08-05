using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameOverManager gameOverManager = GameOverManagerUtil.GetGameOverManager();
        if (other.CompareTag(TagConstants.GAME_OVER_COLLIDER))
        {
            Debug.Log("Triggered with Player2, triggering Game Over.");
            gameOverManager.TriggerGameOver();
        }
    }
}
