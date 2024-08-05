using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 20000f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        Debug.Log("Take Damage: " + amount);
        currentHealth -= amount;
        Debug.Log("Current Health: " +  currentHealth);
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
