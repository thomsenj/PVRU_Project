using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManagerUtil : MonoBehaviour
{
    public static GameOverManager GetGameOverManager()
    {
        GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
        if (gameOverManager == null)
        {
            Debug.LogError("No GameOverManager found in the scene.");
            throw new MissingGameOverManagerException();
        }
        return gameOverManager;
    }

    private class MissingGameOverManagerException : System.Exception
    {
        public MissingGameOverManagerException() : base("No GameOverManager found in the scene.")
        {
        }
    }
}
