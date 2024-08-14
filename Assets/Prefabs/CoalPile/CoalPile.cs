using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class CoalPile : NetworkBehaviour
{
    [Networked]
    public int coalAmount { get; set; } = 10;

    public GameObject coalPrefab; 
    private List<GameObject> list;

    void Start() 
    {
        list = new List<GameObject>();
        coalPrefab.SetActive(false);
        for (int i = 0; i < 2; i++) 
        {
            GameObject coal = Runner.Spawn(coalPrefab);
            coal.SetActive(false);  
            list.Add(coal);
        }
    }

    public void AddCoal(GameObject gameObject) 
    {
        list.Add(gameObject);
    }

    public GameObject TakeCoal()
    {
        if (coalAmount > 0)
        {
            coalAmount--;
            if (list.Count > 0) 
            {
                GameObject coal = list[0];
                list.RemoveAt(0);
                coal.SetActive(true);  
                return coal;
            } 
            else 
            {
                GameObject coal = Runner.Spawn(coalPrefab);
                coal.SetActive(true); 
                return coal;
            }
        }
        return null;
    }
}
