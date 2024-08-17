using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 20000;
    private int currentHealth;
    private HealthbarController ui;

    void Start()
    {
        currentHealth = maxHealth;
        try {
            ui = GameObject.Find(GeneralConstants.HEALTH_BAR).GetComponent<HealthbarController>();
            if(ui != null){
                ui.SetMaxValue(maxHealth);
                ui.SetValue(maxHealth);
            }
        } catch {
            
        }
    }

    public void TakeDamage(float amount)
    {
        int amountInt = (int) amount;
        currentHealth -= amountInt;
        if(ui != null){
            if(currentHealth > 0) {
                ui.ApplyDelta(-amountInt);
            } else {
                ui.SetValue(0);
            }
        }
        if (currentHealth <= 0)
        {
            Die();
        } else {
            triggerEms();
        }
    }

    // trigger ems
    private void triggerEms()
    {
    //    check if there is a script attached to the same game object with name "EMSAdapter"
        if (gameObject.GetComponent<EMSAdapter>() != null)
        {
            // call the "triggerEms" method of the EMSAdapter component
            gameObject.GetComponent<EMSAdapter>().SendImpulseChannel1(500);
        }
    }


    void Die()
    {
        Debug.Log("Player Died!");
        GameOverManager gameOverManager = GameOverManagerUtil.GetGameOverManager();
        gameOverManager.TriggerGameOver();
    }
}
