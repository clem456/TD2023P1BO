using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;

public class Buy : MonoBehaviour
{
    internal GameHandler map;

    internal Placement placement;

    /*
    private void OnMouseEnter()
    {
        ShowcasePrice(true);
    }

    private void OnMouseExit()
    {
        ShowcasePrice(false);
    }

    void ShowcasePrice(bool canShowcase)
    {
        if (canShowcase)
        {

        }
        else
        {

        }
    }*/

    public void BuyAlly()
    {
        Transform allyBody = Instantiate(Resources.Load<Transform>($"Allies/{transform.name}"));
        allyBody.name = transform.name;
        allyBody.parent = map.transform.Find("Allies");

        placement.SetAlly(allyBody.GetComponent<Ally>());
    }
}
