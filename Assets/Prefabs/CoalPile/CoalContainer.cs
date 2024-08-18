using UnityEngine;
using Fusion;
using Fusion.XRShared.Demo;
using System.Collections;

public class CoalContainer : NetworkBehaviour
{
    private ResourceSpawnerPrefab resourceSpawnerPrefab;
    private int currentHits = 0;
    [SerializeField] private int maxHitsToKill;
    [SerializeField] private ResourceContainer resourceContainer;

    [SerializeField] private float bobbingAmount = 0.1f; // How much the object should bob up and down
    [SerializeField] private float bobbingSpeed = 1.0f;  // How fast the object should bob up and down
    [SerializeField] private float rotationSpeed = 30.0f; // Rotation speed around its own axis

    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(BobAndRotate());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            if (maxHitsToKill <= currentHits)
            {
                HandleSpawn();
                resourceSpawnerPrefab = GameObject.FindGameObjectWithTag(TagConstants.WORLD_MANAGER).GetComponent<ResourceSpawnerPrefab>();
                resourceSpawnerPrefab.Despawn(gameObject);
            }
            else
            {
                currentHits++;
            }
        }
    }

    private void HandleSpawn()
    {
        if (resourceContainer == ResourceContainer.COAL)
        {
            AddCoal addCoal = GameObject.FindGameObjectWithTag(TagConstants.COAL_PILE).GetComponent<AddCoal>();
            addCoal.spawnCoal();
        }
        if (resourceContainer == ResourceContainer.WATER)
        {
            WaterBucket w = GameObject.FindGameObjectWithTag(TagConstants.RESOURCE_TOOL).GetComponent<WaterBucket>();
            w.FillUp();
        }
    }

    private IEnumerator BobAndRotate()
    {
        while (true)
        {
            // Bobbing up and down
            float newY = initialPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount;
            transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);

            // Rotating around the Y-axis
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            yield return null;
        }
    }
}

public enum ResourceContainer
{
    COAL,
    WATER
}