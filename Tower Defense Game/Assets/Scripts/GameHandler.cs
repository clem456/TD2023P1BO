using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Events;
using UnityEditor.PackageManager;
using System.Collections.Specialized;
using System.ComponentModel.Design;

public class GameHandler : MonoBehaviour
{
    [SerializeField] internal int Wave = 0;

    [SerializeField] internal int EnemyIncrement;
    [SerializeField] internal int CoinIncrement;

    internal Placement placement;

    internal int RoundEnemies;
    internal int RoundCoins;

    private List<Enemy> Enemies;

    private void Start()
    {
        this.placement = gameObject.AddComponent<Placement>();

        for (int i = 0; i < transform.Find("Grids").childCount; i++)
        {
            GameObject grid = transform.Find("Grids").GetChild(i).gameObject;

            grid.AddComponent<Grid>();
        }

        SpawnEnemies(this.RoundEnemies);        
    }

    private void SpawnEnemies(int Amount)
    {
        for (int i = 0; i < Amount; i++)
        {
            // This code is just for test now
            Enemy enemy = new Enemy();

            this.Enemies.Add(enemy);
        }
    }

    internal List<Transform> GetWaypoints()
    {
        List<Transform> waypoints = new();

        for (int i = 0; i < transform.Find("Path").childCount; i++)
        {
            Transform waypoint = transform.Find("Path").GetChild(i);

            waypoints.Add(waypoint);
        }

        return waypoints;
    }

    internal void WaveCompleted()
    {
        this.Wave += 1;
        this.RoundEnemies += this.EnemyIncrement;
        this.RoundCoins += this.CoinIncrement;

        this.SpawnEnemies(this.RoundEnemies);
    }

    internal Enemy GetNearbyEnemy(Ally ally)
    {
        Enemy mostNearbyEnemy = null;

        float mostNearbyFloat = 0f;

        bool FirstLoop = true;

        foreach(Enemy enemy in this.Enemies)
        {
            if (FirstLoop) { mostNearbyFloat = Vector2.Distance(ally.transform.position, enemy.transform.position); mostNearbyEnemy = enemy; FirstLoop = !FirstLoop; }
            if (mostNearbyEnemy = enemy) { continue; }

            if (mostNearbyFloat > Vector2.Distance(ally.transform.position, enemy.transform.position))
            {
                mostNearbyFloat = Vector2.Distance(ally.transform.position, enemy.transform.position);
                mostNearbyEnemy = enemy;
            }
        }

        return mostNearbyEnemy;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);
        }
    }
}
