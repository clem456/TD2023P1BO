using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class Info : MonoBehaviour
{
    private Transform HP;

    private void Start()
    {
        HP = transform.Find("HP");

        Tower.HealthChanged += showcaseHealth;
    }

    private void showcaseHealth(int health)
    {
        if (health > 0)
        {
            HP.Find("text").GetComponent<TextMeshProUGUI>().text = Convert.ToString(health);
        }
        else
        {
            HP.Find("text").GetComponent<TextMeshProUGUI>().text = "0";
        }
    }
}
