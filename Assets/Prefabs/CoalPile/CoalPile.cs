using UnityEngine;

public class CoalPile : MonoBehaviour
{
    public int coalAmount = 10;
    public GameObject coalPrefab; // Das Kohleklumpen-Prefab

    public GameObject TakeCoal()
    {
        if (coalAmount > 0)
        {
            coalAmount--;
            GameObject coal = Instantiate(coalPrefab);
            return coal;
        }
        return null;
    }
}
