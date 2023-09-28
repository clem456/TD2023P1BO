using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Grid : MonoBehaviour
{
    GameHandler map;

    Placement placement;

    internal Vector3 Size;
    internal Vector3 Position;

    private void Enter()
    {
        if (!this.placement) { return; }
        this.placement.currentGrid = this;
    }

    private void Leave()
    {
        if (!this.placement) { return; }
        if (this.placement.currentGrid == null) { return; }
        if (this.placement.currentGrid == this)
        {
            this.placement.currentGrid = null;
        }
    }
    internal Dictionary<string, Vector2> GetGridInfo()
    {
        return new Dictionary<string, Vector2>()
        {
            {"Grid", new(1,1)},
            {"Begin", new(this.Position.x - this.Size.x / 2, this.Position.y - this.Size.y / 2)},
            {"End", new(this.Position.x + this.Size.x / 2, this.Position.y + this.Size.y / 2)}
        };
    }

    void Start()
    {
        this.map = transform.parent.parent.GetComponent<GameHandler>();
        this.placement = map.placement;

        this.Position = transform.position;
        this.Size = transform.localScale;
    }

    private void Update()
    {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Dictionary<string, Vector2> info = this.GetGridInfo();

        if (MousePos.x < info["Begin"].x || MousePos.x > info["End"].x) { Leave();  return; }
        if (MousePos.y < info["Begin"].y || MousePos.y > info["End"].y) { Leave(); return; }

        Enter();
    }
}
