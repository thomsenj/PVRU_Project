using UnityEngine;
using System.Collections.Generic;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance { get; private set; }

    public GameObject bulletPrefab;         // Prefab der Bullet
    public int initialPoolSize = 20;        // Anfangsgröße des Pools

    private Queue<BulletController> bulletPool = new Queue<BulletController>();

    void Awake()
    {
        // Singleton-Instanz initialisieren
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Pool initialisieren
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewBullet();
        }
    }

    // Methode zum Erstellen einer neuen Bullet
    private void CreateNewBullet()
    {
        GameObject bulletObject = Instantiate(bulletPrefab);
        BulletController bulletController = bulletObject.GetComponent<BulletController>();
        bulletObject.SetActive(false);
        bulletPool.Enqueue(bulletController);
        bulletObject.transform.SetParent(transform);
    }

    // Methode zum Abrufen einer Bullet aus dem Pool
    public BulletController GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            CreateNewBullet();
        }

        BulletController bullet = bulletPool.Dequeue();
        return bullet;
    }

    // Methode zum Zurückgeben einer Bullet in den Pool
    public void ReturnBulletToPool(BulletController bullet)
    {
        bulletPool.Enqueue(bullet);
    }
}