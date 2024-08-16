using UnityEngine;
using Fusion;

public class BulletPoolManager : NetworkBehaviour
{
    public GameObject bulletPrefab;
    private GameObject bulletParent;

    void Awake()
    {
        // create a parent object for the bullets
        bulletParent = new GameObject("BulletParent");
        bulletParent.transform.SetParent(null);
    }

    // Methode zum Erstellen einer neuen Bullet
    public BulletController SpawnBullet(Vector3 startPosition)
    {
        NetworkObject bulletObject = Runner.Spawn(bulletPrefab, startPosition);
        BulletController bulletController = bulletObject.GetComponent<BulletController>();
        bulletObject.transform.SetParent(bulletParent.transform);
        return bulletController;
    }

    // Methode zum Zurückgeben einer Bullet in den Pool
    public void Despawn(BulletController bullet)
    {
        Runner.Despawn(bullet.GetComponent<NetworkObject>());
    }
}