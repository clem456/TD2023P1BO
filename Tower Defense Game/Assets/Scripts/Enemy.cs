using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    internal string Name;

    [SerializeField] private float speed;
    [SerializeField] private float maxHealth;

    private int WaypointIndex = 0;

    private float health = 0f;
    private float reachedDistance = .2f;

    private GameHandler map;

    private void Start()
    {
        this.health = maxHealth;

        GameHandler map = transform.parent.parent.GetComponent<GameHandler>();

        List<Transform> waypoints = map.GetWaypoints();

        transform.position = waypoints[0].position;
    }
    private void Move()
    {
        List<Transform> waypoints = map.GetWaypoints();

        if (waypoints == null) { return; }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[this.WaypointIndex].position, Time.deltaTime * this.speed);

        if (Vector2.Distance(transform.position, waypoints[this.WaypointIndex].position) <= this.reachedDistance)
        {
            this.WaypointIndex += 1;
        }
    }

    internal void TakeDamage(float dmg)
    {
        this.health -= dmg;

        if (this.health < 0)
        {
            this.health = 0;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        this.Move();
    }
}
