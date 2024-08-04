using UnityEngine;
using System.Collections;

public class Resource : MonoBehaviour
{
    public ResourceType resourceType;
    public int amount = 1;
    public int hitsToHarvest = 3; 
    public Color hitColor = Color.red; 
    public float colorChangeDuration = 0.2f; 

    private int currentHits = 0;
    private PlayerInventory playerInventory;
    private Pickaxe pickaxe;
    private Renderer resourceRenderer;
    private Color originalColor;

    void Start()
    {
        GameObject player = GameObject.FindWithTag(TagConstants.Player2Name);
        pickaxe = player.GetComponent<Pickaxe>();
        playerInventory = player .GetComponent<PlayerInventory>();
        resourceRenderer = GetComponent<Renderer>();
        if (resourceRenderer != null)
        {
            originalColor = resourceRenderer.material.color;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("In Trigger Enter.");
        try
        {
            GameOverManager gameOverManager = FindObjectOfType<GameOverManager>();
            if (gameOverManager == null)
            {
                Debug.LogError("No GameOverManager found in the scene.");
                return;
            }
            if (other.CompareTag(TagConstants.GAME_OVER_COLLIDER))
            {
                Debug.Log("Triggered with Player2, triggering Game Over.");
                gameOverManager.TriggerGameOver();
            }
            if (other.CompareTag(TagConstants.RESOURCE_TOOL))
            {
                if (pickaxe.IsSwinging())
                {
                    pickaxe.setSwinging(false);
                    Debug.Log("Start Harvest.");
                    Harvest();
                }
            }
        }
        catch
        {
            Debug.LogError("An error occurred during trigger handling.");
        }
    }

    public void Harvest()
    {
        currentHits++;
        Debug.Log($"Resource hit {currentHits} times.");

        if (resourceRenderer != null)
        {
            StartCoroutine(ChangeColorTemporarily());
        }

        if (currentHits >= hitsToHarvest)
        {
            playerInventory.AddResource(resourceType, amount);
            Destroy(gameObject);
        }
    }

    private IEnumerator ChangeColorTemporarily()
    {
        resourceRenderer.material.color = hitColor;
        yield return new WaitForSeconds(colorChangeDuration);
        resourceRenderer.material.color = originalColor;
    }

}
