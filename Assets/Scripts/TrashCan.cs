using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TriggerZone>().OnEnterEvent.AddListener(InsideTrash);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void InsideTrash(GameObject gameObject)
    {
        Destroy(gameObject);
    }
}
