using UnityEngine;
using System.Collections.Generic;

public class BulletPoolManager : MonoBehaviour
{
    public static BulletPoolManager Instance { get; private set; }

    public GameObject bulletPrefab;         // Prefab der Bullet
    public int initialPoolSize = 5;        // Anfangsgröße des Pools

    private Queue<BulletController> bulletPool = new Queue<BulletController>();
    private GameObject bulletParent;

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
        // create a parent object for the bullets
        bulletParent = new GameObject("BulletParent");
        bulletParent.transform.SetParent(null);
    }

    // Methode zum Erstellen einer neuen Bullet
    private void CreateNewBullet()
    {
        GameObject bulletObject = Instantiate(bulletPrefab);
        BulletController bulletController = bulletObject.GetComponent<BulletController>();
        bulletObject.SetActive(false);
        bulletPool.Enqueue(bulletController);
        bulletObject.transform.SetParent(bulletParent.transform);
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