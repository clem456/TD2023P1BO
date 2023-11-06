using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Placement : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject part;

    internal GameHandler map;

    private Ally ally;

    public Grid currentGrid;

    void Start()
    {
        map = gameObject.GetComponent<GameHandler>();
    }

    internal void SetAlly(Ally newAlly)
    {
        ally = newAlly;

        if (!ally)
        {
            return;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (ally)
            {
                ally.Place();
            }
        }
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject newAlly = Instantiate(Resources.Load<GameObject>("Allies/Minim"));
            newAlly.transform.parent = map.transform.Find("Allies");
            ally = newAlly.GetComponent<Ally>();
        }*/

        if (!ally) 
        {
            SetAlly(null);
            return;
        }
        
        if (ally.placed)
        {
            ally = null;
        }
        else
        {
            ally.Move(this.currentGrid);
        }
    }
}
