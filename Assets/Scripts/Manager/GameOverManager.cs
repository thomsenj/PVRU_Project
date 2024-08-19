using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameOverManager : NetworkBehaviour
{

    private ScoreManager scoreManager;

    public bool testTrigger = false;

    private void Start()
    {
        scoreManager = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ScoreManager>();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (testTrigger)
        {
            testTrigger = false;
            TriggerGameOver(); 
        }
    }

    public void TriggerGameOver()
    {
        if (HasStateAuthority) // should be true for all players as we are in shared mode
        {
            scoreManager.stopScoring();
            Invoke("RpcRestartGameForAll", 3f); 
        }
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RpcRestartGameForAll()
    {
        RestartGame();
    }

    private void RestartGame()
    {

        SceneManager.LoadScene("MainMenu");
        Runner.UnloadScene(SceneRef.FromIndex(1)); //Runner.UnloadScene(SceneRef.FromIndex(1));
    }
}
