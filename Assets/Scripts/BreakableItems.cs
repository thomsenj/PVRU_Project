using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItems : MonoBehaviour
{
    public List<GameObject> breakablePieces;
    public float timeToBreak = 2;
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        DisableBreakablePieces();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisableBreakablePieces()
    {
        foreach (var item in breakablePieces)
        {
            item.SetActive(false);
        }
    }

    public void BreakItem()
    {
        timer += Time.deltaTime;

        if (timer > timeToBreak)
        {
            foreach (var item in breakablePieces)
            {
                item.SetActive(true); // Showing all disabled pieces
                item.transform.parent = null;  // But disable Parent component
            }
            gameObject.SetActive(false);
        }
    }
}
