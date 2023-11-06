using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ally : MonoBehaviour
{
    [SerializeField] internal int price;
    [SerializeField] internal float health;
    [SerializeField] internal float damage;
    [SerializeField] internal float range;
    [SerializeField] internal float cooldown;
    [SerializeField] internal float speed;

    internal bool placed = false;

    internal Vector2 Grid;
    internal Vector2 GridPos;

    private GameHandler map;

    private Vector2 GridSize = new(1,1);

    private bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false;
        map = transform.parent.parent.GetComponent<GameHandler>();
    }

    internal IEnumerator Attack()
    {
        Enemy enemy = GetNearbyEnemy();

        if (enemy)
        {
            if (Vector2.Distance(transform.position, enemy.transform.position) <= range)
            {
                Vector3 difference = enemy.transform.position - transform.position;
                difference.Normalize();

                float rot_z = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

                if (canShoot)
                {
                    canShoot = !canShoot;

                    Transform projectile = Instantiate(Resources.Load<Transform>(@$"Notes/{transform.name}"), transform);
                    projectile.position = transform.position;

                    Note note = projectile.AddComponent<Note>();
                    note.Target = enemy;
                    note.damage = damage;
                    note.activate = true;

                    yield return new WaitForSeconds(cooldown);

                    canShoot = !canShoot;
                }
            }
        }
    }

    internal void Move(Grid grid)
    {
        if (this.placed) { return; }
        if (!grid)
        {
            return;
        }

        Dictionary<string, Vector2> info = grid.GetGridInfo();

        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (MousePos.x < info["Begin"].x || MousePos.x > info["End"].x) { return; }
        if (MousePos.y < info["Begin"].y || MousePos.y > info["End"].y) { return; }

        Vector2 GridAmount = new(grid.Size.x / info["Grid"].x, grid.Size.y / info["Grid"].y);

        Vector2 Cal = new(
            Mathf.Floor(Mathf.Clamp((info["End"].x - MousePos.x) / info["Grid"].x, 0f, GridAmount.x - (this.GridSize.x * info["Grid"].x / info["Grid"].x))),
            Mathf.Floor(Mathf.Clamp((info["End"].y - MousePos.y) / info["Grid"].y, 0f, GridAmount.y - (this.GridSize.y * info["Grid"].y / info["Grid"].y)))
        );

        Vector2 GridPos = new
        (
            info["End"].x - (this.GridSize.x * info["Grid"].x / 2) - info["Grid"].x * Cal.x,
            info["End"].y - (this.GridSize.y * info["Grid"].y / 2) - info["Grid"].y * Cal.y
        );

        transform.position = GridPos;

        this.Grid = Cal;
        this.GridPos = GridPos;
    }

    internal Enemy GetNearbyEnemy()
    {
        Enemy mostNearbyEnemy = null;

        float mostNearbyFloat = 0f;

        bool FirstLoop = true;

        foreach (Transform enemy in map.Enemies)
        {
            Enemy EnemyClass = enemy.GetComponent<Enemy>();

            if (FirstLoop) { 
                mostNearbyFloat = Vector2.Distance(transform.position, enemy.transform.position); 
                mostNearbyEnemy = EnemyClass; 
                FirstLoop = !FirstLoop;
            }
            if (mostNearbyEnemy == EnemyClass) { continue; }

            if (mostNearbyFloat > Vector2.Distance(transform.position, enemy.position))
            {
                mostNearbyFloat = Vector2.Distance(transform.position, enemy.position);
                mostNearbyEnemy = EnemyClass;
            }
        }

        return mostNearbyEnemy;
    }

    internal void Place()
    {
        if (placed) { return; }
        placed = !placed;

        transform.position = GridPos;

        enabled = true;
    }

    private void Update()
    {
        if (!canShoot) { return; }

        if (placed)
        {
            StartCoroutine(Attack());
        }
        else
        {
            enabled = false;
        }
    }
}
