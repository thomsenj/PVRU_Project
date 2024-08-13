using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 20000;
    private int currentHealth;
    private HealthbarController ui;

    void Start()
    {
        currentHealth = maxHealth;
        ui = GameObject.Find(GeneralConstants.HEALTH_BAR).GetComponent<HealthbarController>();
        ui.SetMaxValue(maxHealth);
        ui.SetValue(maxHealth);
    }

    public void TakeDamage(float amount)
    {
        int amountInt = (int) amount;
        currentHealth -= amountInt;
        if(currentHealth > 0) {
            ui.ApplyDelta(-amountInt);
        } else {
            ui.SetValue(0);
        }
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died!");
        GameOverManager gameOverManager = GameOverManagerUtil.GetGameOverManager();
        gameOverManager.TriggerGameOver();
    }
}
