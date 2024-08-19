using UnityEngine;

public class MenuRouter : MonoBehaviour
{
    private const string SceneCounterKey = "SceneCounter"; // Schlüssel für PlayerPrefs

    // Startwert für die Szene (z.B. Game1)
    private int sceneCounter;

    void Start()
    {
        // Lade den aktuellen Szenenzähler aus den PlayerPrefs, standardmäßig auf 1 gesetzt
        sceneCounter = PlayerPrefs.GetInt(SceneCounterKey, 1);
    }

    public void StartGame1()
    {
        // Erstelle den Szenennamen basierend auf dem Zähler
        string sceneName = $"Game{sceneCounter}";

        // Lade die Szene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);

        // Erhöhe den Zähler für das nächste Mal
        sceneCounter++;

        // Speichere den neuen Zählerwert in den PlayerPrefs
        PlayerPrefs.SetInt(SceneCounterKey, sceneCounter);
        PlayerPrefs.Save();
    }

    public void StartGame2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("AllBiomes");
    }
}