using UnityEngine;
using System.Collections;
using UnityEngine.XR.Interaction.Toolkit;

public class RevolverController : MonoBehaviour
{
    public Transform muzzle;                    // Die Mündung des Revolvers, wo die Kugel herauskommt
    public float recoilAmount = 0.25f;          // Die Stärke des Rückstoßes
    public float recoilSpeed = 25f;             // Die Geschwindigkeit der Rückstoßbewegung
    public BulletPoolManager bulletPool;        // Referenz zum Bullet-Pool-Manager
    public ParticleSystem shellEjectParticle;   // Partikelsystem für den Hülsenauswurf

    // Revolver-Teile
    public Transform cylinder;                  // Zylinder des Revolvers
    public Transform hammer;                    // Hammer des Revolvers
    public Transform trigger;                   // Abzug des Revolvers

    // private Vector3 originalPosition;           // Die ursprüngliche Position des Revolvers
    // private Quaternion originalRotation;        // Die ursprüngliche Rotation des Revolvers
    private bool isRecoiling = false;           // Gibt an, ob der Revolver sich im Rückstoß befindet
    private Vector3 recoilPosition;             // Die Position, zu der der Revolver sich beim Rückstoß bewegt
    private Quaternion recoilRotation;          // Die Rotation, zu der der Revolver sich beim Rückstoß dreht
    private bool canShoot = true;               // Gibt an, ob ein Schuss abgegeben werden kann
    // private XRGrabInteractable grabInteractable;


    void Start()
    {
        // grabInteractable = GetComponent<XRGrabInteractable>();

        // grabInteractable.selectEntered.AddListener(OnGrabbed);

        // grabInteractable.selectExited.AddListener(OnReleased);
    }

    // private void OnGrabbed(SelectEnterEventArgs args)
    // {
    //     canShoot = true;
    // }

    // Wird aufgerufen, wenn das Objekt losgelassen wird
    // private void OnReleased(SelectExitEventArgs args)
    // {
    //     canShoot = false;
    // }


    // void Update()
    // {
    //     if (Input.GetMouseButtonDown(0) && canShoot) // Linksklick zum Schießen
    //     {
    //         Shoot();
    //     }

    // if (isRecoiling)
    // {
    //     // Bewege den Revolver zu seiner Rückstoßposition
    //     transform.localPosition = Vector3.Lerp(transform.localPosition, recoilPosition, Time.deltaTime * recoilSpeed);
    //     transform.localRotation = Quaternion.Slerp(transform.localRotation, recoilRotation, Time.deltaTime * recoilSpeed);

    //     // Wenn der Revolver die Rückstoßposition erreicht hat, bewege ihn zurück
    //     if (Vector3.Distance(transform.localPosition, recoilPosition) < 0.01f && Quaternion.Angle(transform.localRotation, recoilRotation) < 0.1f)
    //     {
    //         isRecoiling = false; // Rückstoß ist abgeschlossen
    //         canShoot = true;
    //     }
    // }
    // else
    // {
    //     // Bewege den Revolver zurück zu seiner ursprünglichen Position
    //     transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * recoilSpeed);
    //     transform.localRotation = Quaternion.Slerp(transform.localRotation, originalRotation, Time.deltaTime * recoilSpeed);

    //     // Wenn der Revolver zurück in seiner Ursprungsposition ist, erlaube den nächsten Schuss
    //     if (Vector3.Distance(transform.localPosition, originalPosition) < 0.01f && Quaternion.Angle(transform.localRotation, originalRotation) < 0.1f)
    //     {
    //         canShoot = true;
    //     }
    // }
    // }

    // public void UpdateOriginalPositionAndRotation()
    // {
    //     originalPosition = transform.localPosition;
    //     originalRotation = transform.localRotation;
    // }

    public void Shoot()
    {
        canShoot = false; // Verhindere weiteres Schießen während des Rückstoßes

        // Hole eine Bullet aus dem Pool und feuere sie ab
        BulletController bullet = bulletPool.GetBullet();
        bullet.Shoot(muzzle.position, muzzle.forward);

        // Spiele das Partikelsystem für den Hülsenauswurf ab
        if (shellEjectParticle != null)
        {
            shellEjectParticle.Play();
        }

        // Initialisiere den Rückstoß und die Animationen
        AnimateRevolver();
        //StartRecoil();
        canShoot = true;
    }

    private void StartRecoil()
    {
        // UpdateOriginalPositionAndRotation();

        isRecoiling = true;

        // Setze die Rückstoßposition und -rotation basierend auf den Recoil-Werten
        // recoilPosition = originalPosition + new Vector3(0, 0, -recoilAmount * 0.5f);
        // recoilRotation = originalRotation * Quaternion.Euler(-recoilAmount * 100f, 0, 0); // Leichte Drehung nach oben (X-Achse)
        recoilPosition = transform.localPosition + new Vector3(0, 0, -recoilAmount * 0.5f);
        recoilRotation = transform.localRotation * Quaternion.Euler(-recoilAmount * 100f, 0, 0); // Leichte Drehung nach oben (X-Achse)
    }

    private void AnimateRevolver()
    {
        // Starte die Animation des Zylinders
        StartCoroutine(AnimateCylinder());

        // Bewege den Hammer zurück und dann nach vorne
        StartCoroutine(AnimateHammer());

        // Bewege den Abzug kurzzeitig
        StartCoroutine(AnimateTrigger());
    }

    private IEnumerator AnimateCylinder()
    {
        float animationTime = 0.25f; // Dauer der Animation
        float elapsedTime = 0f;
        Quaternion initialRotation = cylinder.localRotation;
        Quaternion finalRotation = initialRotation * Quaternion.Euler(0, 0, 60f); // 60 Grad weiter drehen

        while (elapsedTime < animationTime)
        {
            cylinder.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        cylinder.localRotation = finalRotation; // Setze die finale Rotation, um sicherzustellen, dass die Animation korrekt endet
    }

    private IEnumerator AnimateHammer()
    {
        float animationTime = 0.2f;
        float elapsedTime = 0f;
        Quaternion initialRotation = hammer.localRotation;
        Quaternion finalRotation = Quaternion.Euler(-45f, 0, 0);

        // Hammer zurückziehen
        while (elapsedTime < animationTime)
        {
            hammer.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Hammer schlagen
        elapsedTime = 0f;
        while (elapsedTime < animationTime)
        {
            hammer.localRotation = Quaternion.Slerp(finalRotation, initialRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        hammer.localRotation = initialRotation;
    }

    private IEnumerator AnimateTrigger()
    {
        float animationTime = 0.2f;
        float elapsedTime = 0f;
        Quaternion initialRotation = trigger.localRotation;
        Quaternion finalRotation = Quaternion.Euler(35f, 0, 0);

        // Trigger drücken
        while (elapsedTime < animationTime)
        {
            trigger.localRotation = Quaternion.Slerp(initialRotation, finalRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Trigger loslassen
        elapsedTime = 0f;
        while (elapsedTime < animationTime)
        {
            trigger.localRotation = Quaternion.Slerp(finalRotation, initialRotation, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        trigger.localRotation = initialRotation;
    }
}