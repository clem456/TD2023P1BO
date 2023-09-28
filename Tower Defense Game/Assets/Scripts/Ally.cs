using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Rendering;

public class Ally : MonoBehaviour
{
    [SerializeField] internal float health;
    [SerializeField] internal float damage;
    [SerializeField] internal float range;

    internal bool placed = false;

    internal Vector2 Grid;
    internal Vector2 GridPos;

    private GameHandler map;

    private Vector2 GridSize = new(1,1);

    // Start is called before the first frame update
    void Start()
    {
        this.map = transform.parent.parent.GetComponent<GameHandler>();
    }

    internal void Attack()
    {
        Enemy enemy = map.GetNearbyEnemy(this);

        enemy.TakeDamage(this.damage);
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

        Vector2 GridAmount = new Vector2(grid.Size.x / info["Grid"].x, grid.Size.y / info["Grid"].y);

        Vector2 Cal = new Vector2(
            Mathf.Floor(Mathf.Clamp((info["End"].x - MousePos.x) / info["Grid"].x, 0f, GridAmount.x - (this.GridSize.x * info["Grid"].x / info["Grid"].x))),
            Mathf.Floor(Mathf.Clamp((info["End"].y - MousePos.y) / info["Grid"].y, 0f, GridAmount.y - (this.GridSize.y * info["Grid"].y / info["Grid"].y)))
        );

        Vector2 GridPos = new Vector2
        (
            info["End"].x - (this.GridSize.x * info["Grid"].x / 2) - info["Grid"].x * Cal.x,
            info["End"].y - (this.GridSize.y * info["Grid"].y / 2) - info["Grid"].y * Cal.y
        );

        transform.position = GridPos;

        this.Grid = Cal;
        this.GridPos = GridPos;
    }

    internal void Place()
    {
        if (this.placed) { return; }
        this.placed = !this.placed;

        transform.position = this.GridPos;
    }
}
