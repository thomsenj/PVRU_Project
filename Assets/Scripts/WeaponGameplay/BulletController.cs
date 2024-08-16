using Fusion;
using UnityEngine;

public class BulletController : NetworkBehaviour
{
    public float speed = 20f;                // Anfangsgeschwindigkeit der Bullet
    public float maxDistance = 100f;         // Maximale Distanz, die die Bullet zurücklegen kann
    public Vector3 gravity = new Vector3(0, -9.81f, 0); // Schwerkraft
    public float dragCoefficient = 0.47f;    // Luftwiderstandsbeiwert für eine kugelförmige Bullet
    public float airDensity = 1.225f;        // Dichte der Luft (kg/m³)
    public float crossSectionalArea = 0.01f; // Querschnittsfläche der Bullet (m²)
    public int damage = 10;               // Schaden, den die Bullet verursacht

    private Vector3 velocity;                // Geschwindigkeit der Bullet
    private float distanceTraveled = 0f;     // Zurückgelegte Distanz
    private TrailRenderer trail;            // Referenz zum Trail-Renderer
    private NetworkObject bulletPrefab;

    void Start()
    {
        trail = GetComponent<TrailRenderer>();
        trail.enabled = false;
    }


        public override void FixedUpdateNetwork()
        {
            base.FixedUpdateNetwork();

          if (gameObject.activeSelf)
        {
            float deltaTime = Time.deltaTime;

            // Berechne die Luftwiderstandskraft
            Vector3 dragForce = -0.5f * dragCoefficient * airDensity * crossSectionalArea * velocity.sqrMagnitude * velocity.normalized;

            // Aktualisiere die Geschwindigkeit der Bullet unter Berücksichtigung von Schwerkraft und Luftwiderstand
            velocity += (gravity + dragForce) * deltaTime;

            // Bewege die Bullet entsprechend ihrer Geschwindigkeit
            Vector3 displacement = velocity * deltaTime;
            gameObject.transform.position += displacement;

            // Aktualisiere die zurückgelegte Distanz
            distanceTraveled += displacement.magnitude;

            // Überprüfen, ob die maximale Distanz erreicht wurde
            if (distanceTraveled >= maxDistance)
            {
                ResetBullet(); // Bullet zurücksetzen und in den Pool zurückgeben
            }
        }
        }

    // Methode zum Starten des Bullet-Flugs
    public void Shoot(Vector3 startPosition, Vector3 shootDirection)
    {
        Runner.Spawn(gameObject, startPosition);
        velocity = shootDirection.normalized * speed; // Setzt die Anfangsgeschwindigkeit
        gameObject.transform.position = startPosition;
        gameObject.transform.SetParent(bulletParent.transform);
        distanceTraveled = 0f; // Setzt die zurückgelegte Distanz zurück
        if(trail != null)
        {
            trail.enabled = true; // Aktiviert den Trail-Renderer
            trail.Clear(); // Löscht den Trail
        }
    }

    void OnTriggerEnter(Collider other)
    {
        ResetBullet(); // Bullet zurücksetzen und in den Pool zurückgeben
        // check if the other object has the component "EnemyAI"
        if (other.GetComponent<EnemyAI>() != null)
        {
            // call the "TakeDamage" method of the EnemyAI component
            other.GetComponent<EnemyAI>().TakeDamage(damage);
        }
    }

    private void ResetBullet()
    {
        Runner.Despawn(gameObject.GetComponent<NetworkObject>());
        trail.enabled = false; // Deaktiviert den Trail-Renderer
    }


}