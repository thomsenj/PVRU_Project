using UnityEngine;

public class DestroyOnCollision : MonoBehaviour
{
    public GameObject coalPileGO;
    private CoalPile coalPile;

    void Start () {
        coalPile = coalPileGO.GetComponent<CoalPile>();
    }   

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(TagConstants.GROUND) | collision.gameObject.CompareTag(TagConstants.TRAIN))
        {
            gameObject.SetActive(false);
            coalPile.AddCoal(gameObject);
        }
    }
}
