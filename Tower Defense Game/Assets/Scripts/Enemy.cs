using Microsoft.Unity.VisualStudio.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public static event Action<Enemy> Died;
    public static event Action<Enemy> ReachedTower;

    internal string Name;

    [SerializeField] internal int threadLevel = 1;

    [SerializeField] internal float maxHealth;
    [SerializeField] internal float towerDamage;

    [SerializeField] private float speed;

    private GameHandler map;
    private List<Transform> waypoints;

    private Transform Healthbar;

    private bool reverse;

    private int WaypointIndex = 0;
    private int frame = 1;

    private float health = 0;
    private float reachedDistance = .2f;
    private float frameTime = 0.15f;
    private float timePassed = 0;

    private void Start()
    {
        health = maxHealth;

        map = transform.parent.parent.GetComponent<GameHandler>();
        waypoints = map.GetWaypoints();

        transform.position = waypoints[0].position;

        Healthbar = Instantiate(Resources.Load<Transform>("UIassets/Healthbar"), transform);
    }

    private void Move()
    {
        if (waypoints == null) { return; }
        if (WaypointIndex == waypoints.Count) { enabled = false; return; }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[WaypointIndex].position, Time.deltaTime * speed);

        if (Vector2.Distance(transform.position, waypoints[WaypointIndex].position) <= reachedDistance)
        {
            WaypointIndex += 1;

            if (WaypointIndex == waypoints.Count)
            {
                ReachedTower?.Invoke(this);
            }
        }
    }

    internal void TakeDamage(float dmg)
    {
        health -= dmg;

        if (health <= 0)
        {
            health = 0;

            Died?.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void ShowcaseHealth()
    {
        UnityEngine.UI.Image Bar = Healthbar.Find("Main").Find("Bar").GetComponent<UnityEngine.UI.Image>();

        float y = Bar.GetComponent<RectTransform>().localScale.y;

        if (health > 0)
        {
            Bar.GetComponent<RectTransform>().localScale = new(health / maxHealth, y, 0);
        }
        else
        {
            Bar.GetComponent<RectTransform>().localScale = new(0, y, 0);
        }
    }

    private void ChangeFrame()
    {
        List<Sprite> frames = new List<Sprite>(Resources.LoadAll<Sprite>(@$"Animations/{transform.name}"));

        if (timePassed >= frameTime)
        {
            if (reverse)
            {
                frame -= 1;
            }
            else
            {
                frame += 1;
            }

            if (frame > frames.Count)
            {
                reverse = true;
                frame = frames.Count-1;
            }
            else if (frame < 1)
            {
                reverse = false;
                frame = 2;
            }

            Debug.Log(frame);

            timePassed = 0;
        }

        GetComponent<SpriteRenderer>().sprite = frames[frame - 1];

        timePassed += Time.deltaTime;
    }

    // Update is called once per frame
    private void Update()
    {
        ChangeFrame();
        ShowcaseHealth();
        Move();
    }
}
