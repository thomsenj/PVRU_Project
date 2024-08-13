using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("In Trigger Enter");
        if (other.CompareTag(TagConstants.GAME_OVER_COLLIDER) || other.CompareTag(TagConstants.TRAIN))
        {
            GameOverManager gameOverManager = GameOverManagerUtil.GetGameOverManager();
            Debug.Log("Triggered with Player2, triggering Game Over.");
            gameOverManager.TriggerGameOver();
        }
    }
}
