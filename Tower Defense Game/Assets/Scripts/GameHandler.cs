using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.Timeline;
using System.Threading.Tasks;
using UnityEditor.ShaderGraph.Drawing;
using System.Linq;
using System;

using Random = UnityEngine.Random;

public class GameHandler : MonoBehaviour
{   
    [SerializeField] internal int EnemyIncrement;
    [SerializeField] internal int CoinIncrement;

    [SerializeField] internal int RoundEnemies;
    [SerializeField] internal int RoundCoins;

    [SerializeField] private float cooldown = 1.5f;

    [SerializeField] private List<int> threadLevels;

    public GameObject ShopUI;
    public GameObject EndScreen;

    internal Placement placement;

    internal List<Transform> Enemies = new();

    private int wave = 0;

    private void Start()
    {
        placement = gameObject.AddComponent<Placement>();

        for (int i = 0; i < transform.Find("Grids").childCount; i++)
        {
            GameObject grid = transform.Find("Grids").GetChild(i).gameObject;

            grid.AddComponent<Grid>();
        }

        Enemy.Died += RemoveEnemy;
        Tower.TowerDestroyed += DestroyTower;

        SetShopUI();
        StartCoroutine(SpawnEnemies());
    }

    internal List<Transform> GetWaypoints()
    {
        List<Transform> waypoints = new List<Transform>();

        for (int i = 0; i < transform.Find("Path").childCount; i++)
        {
            Transform waypoint = transform.Find("Path").GetChild(i);

            waypoints.Add(waypoint);
        }

        return waypoints;
    }

    internal IEnumerator SpawnEnemies()
    {
        List<Transform> AllEnemies = new List<Transform>(Resources.LoadAll<Transform>("Enemies"));

        for (int i = 0; i < AllEnemies.Count; i++)
        {
            Enemy enemy = AllEnemies[i].GetComponent<Enemy>();

            if (enemy.threadLevel > threadLevels[wave])
            {
                AllEnemies.Remove(AllEnemies[i]);
            }
        }

        for (int i = 0; i < RoundEnemies; i++)
        {
            Transform random = AllEnemies[Random.RandomRange(0, AllEnemies.Count)];

            Transform enemy = Instantiate(random, transform.Find("Enemies"));
            enemy.name = random.name;
            Enemies.Add(enemy);

            yield return new WaitForSeconds(cooldown);
        }
    }

    internal void WaveCompleted(bool firstTime)
    {
        wave += 1;
        RoundEnemies += EnemyIncrement;
        RoundCoins += CoinIncrement;

        StartCoroutine(SpawnEnemies());
    }

    private void SetShopUI()
    {
        GameObject[] Allies = Resources.LoadAll<GameObject>("Allies");

        int increment = 0;

        foreach (GameObject ally in Allies)
        {
            Sprite sprite = ally.GetComponent<SpriteRenderer>().sprite;

            Transform Holder = Instantiate(Resources.Load<Transform>("UIassets/Holder"));

            Holder.name = ally.name;
            Holder.SetParent(ShopUI.transform.Find("Frame"));
            Holder.localScale = new(0.175f, 0.23f, 5.3f);
            Holder.Find("AllyName").GetComponent<TextMeshProUGUI>().text = ally.name;
            Holder.Find("Icon").GetComponent<Image>().sprite = sprite;
            Holder.GetComponent<RectTransform>().localPosition = new(-50 + 10 + (22.5f * increment), 37.5f, 0);

            Buy buy = Holder.GetComponent<Buy>();

            buy.placement = placement;
            buy.map = this;

            Button button = Holder.AddComponent<Button>();
            button.onClick.AddListener(buy.BuyAlly);

            increment += 1;
        }
    }

    private void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy.transform);

        if (Enemies.Count == 0)
        {
            WaveCompleted(false);
        }
    }

    private void DestroyTower(Tower tower)
    {
        EndScreen.active = true;
    }
}
