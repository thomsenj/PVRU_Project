using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public string scoreText; 
    public float score;     
    public float scoreIncreaseRate = 1f;  
    private bool isScoring;
    void Start()
    {
        isScoring = true;
        score = 0f;
    }

    void Update()
    {
        if(isScoring)
        {
            score += scoreIncreaseRate * Time.deltaTime;
        }
    }

    public void AddBonusPoints(int bonusPoints)
    {
        if(isScoring) {
            score += bonusPoints;
        }
    }
    public int GetEnemyCount()
    {
        int baseEnemyCount = 5;
        int additionalEnemies = (int)(score / 100f);
        return baseEnemyCount + additionalEnemies;
    }

    public void stopScoring() {
        isScoring = false;
    }
}
