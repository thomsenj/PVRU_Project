using System.Collections;
using UnityEngine;

public class ResourceExplosion : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystemPrefab;

    private void Start()
    {
        if (particleSystemPrefab.isPlaying)
        {
            particleSystemPrefab.Stop();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagConstants.GROUND))
        {
            StartCoroutine(DestroyAfterEffect());
        }
    }

    private IEnumerator DestroyAfterEffect()
    {
        particleSystemPrefab.Play();
        yield return new WaitForSeconds(particleSystemPrefab.main.duration);
        Destroy(gameObject);
    }
}
