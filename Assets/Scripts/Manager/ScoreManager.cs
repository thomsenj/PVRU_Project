using Fusion;
using UnityEngine;

public class ScoreManager : NetworkBehaviour
{
    public string scoreText;

    [Networked]
    public float score { get; set; } = 0;
    public float scoreIncreaseRate = 2f;  

    private CollectableBankController scoreUI;
    private bool isScoring;

    public void Start()
    {
        isScoring = true;
        try
        {
            scoreUI = GameObject.Find(GeneralConstants.SCORE_COUNTER).GetComponent<CollectableBankController>();
            scoreUI.SetCount(0);
        }
        catch
        {
            // Debug.LogError("No Score UI");
        }
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (isScoring)
        {
            score += scoreIncreaseRate * Time.deltaTime;
            try {
                scoreUI.SetCount((int) score);
            } catch {
                //Debug.LogError("No Score UI");
            }
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

    public void stopScoring(){
        isScoring = false;
    }
}
