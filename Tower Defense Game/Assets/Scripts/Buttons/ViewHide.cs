using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewHide : MonoBehaviour
{
    private bool View = true;

    public void ViewAndHide()
    {
        View = !View;

        if (View)
        {
            RectTransform rect = transform.parent.GetComponent<RectTransform>();

            rect.position = new(rect.position.x, -550);

            View = !View;
        }
        else
        {

            RectTransform rect = transform.parent.GetComponent<RectTransform>();

            rect.position = new(rect.position.x, -120);

            View = !View;
        }
    }
}
