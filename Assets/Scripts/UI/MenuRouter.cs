using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRouter : MonoBehaviour
{

    // main game scene
    public string gameSceneName = "Game";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void StartGame()
    {
        // reload scene "allBioms"
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameSceneName);
    }
}
