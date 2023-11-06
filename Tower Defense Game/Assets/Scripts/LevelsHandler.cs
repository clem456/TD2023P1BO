using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LevelsHandler : MonoBehaviour
{
    private void Start()
    {
        Object[] files = Resources.LoadAll($"Levels");

        foreach(var child in files)
        {
            Transform frame = Instantiate(Resources.Load<Transform>("UIassets/Level"));

            if ((Sprite)child)
            {
                frame.Find("Icon").GetComponent<Image>().sprite = child.GetComponent<Sprite>();
                frame.GetComponent<RectTransform>().position = new(-500, 0);
                frame.Find("Icon").AddComponent<Button>().onClick.AddListener(clicked);
            }
        }
    }

    private void clicked()
    {

    }
}
