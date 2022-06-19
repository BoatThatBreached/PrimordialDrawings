using System.Collections.Generic;
using System.Linq;
using Game_Objects;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject terrainPref;

    public TMP_Text currentSpawn;
    public GameObject levelContainer;
    public GameObject toLearnContainer;
    public static bool IsPaused;
    public Player player;
    public List<SavedEntry> spawned;
    public float treeHeight;
    public CircleLayoutGroup wooden;

    private void Awake()
    {
        IsPaused = false;
        spawned = new List<SavedEntry>();
        LoadLevel();
        PlayerInfo.Load();
        wooden.Init(PlayerInfo.WoodenPiecesLeft);
        player.transform.position = new Vector3(4, 9, -15) +
                                    FindObjectsOfType<Fire>()
                                        .First(f => f.fireIndex == PlayerInfo.FireIndex)
                                        .transform.position;
        if (PlayerInfo.Learned.Contains(""))
            PlayerInfo.Learned.Remove("");
        foreach (var platform in PlayerInfo.Platforms)
            AddSavedTerrains(platform);
        foreach (var pos in PlayerInfo.SpawnedSprouts)
            SpawnSprout(pos);
        foreach (var skull in PlayerInfo.Skulls)
            SpawnSkull(skull);
    }

    public void Spawn(string type, Vector3 pos)
    {
        if ((type == "saddle" || type == "boat" || type == "spear"|| type == "sprout") && PlayerInfo.WoodenPiecesLeft == 0)
            return;
        
        var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/your_{type}"), transform.parent)
            .GetComponent<SavedEntry>();
        obj.transform.position = pos;
        if (type == "saddle" || type == "boat" || type == "spear"|| type == "sprout")
        {
            wooden.Select(obj);
            PlayerInfo.WoodenPiecesLeft--;
            PlayerInfo.Save();
        }
        var finish = type switch
        {
            "saddle" => () =>
            {
                player.held = obj.transform;
                player.held.SetParent(transform);
                player.held.localScale = 0.5f * Vector3.one;
                player.held.localPosition = new Vector3(1.5f, 1.5f, 0);
                player.saddled = true;
            },
            "boat" => () =>
            {
                var water = FindObjectOfType<Water>();
                if (water == null || water.boat != null)
                    return;
                var closest = water.waterPointsContainer.GetChild(0);
                for (var j = 1; j < water.waterPointsContainer.childCount; j++)
                    if (Vector2.Distance(
                            water.waterPointsContainer.GetChild(j).position,
                            player.transform.position)
                        < Vector2.Distance(
                            closest.position,
                            player.transform.position))
                        closest = water.waterPointsContainer.GetChild(j);
                obj.transform.position = closest.position + Vector3.up;
                water.boat = obj.transform;
            },
            "sprout" => () =>
            {
                PlayerInfo.SpawnedSprouts.Add(pos);
                spawned.Add(obj);
                obj.treeHeight = treeHeight;
            },
            "spear" => () =>
            {
                player.held = obj.transform;
                player.held.SetParent(transform);
                //player.held.localScale = 0.5f * Vector3.one;
                player.held.localPosition = new Vector3(1.5f, 1.5f, 0);
                player.armed = true;
            },

            _ => PlayerInfo.Pass
        };
        obj.Animate(finish);
    }

    private void SpawnSkull(Vector3 pos)
    {
        var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/skull"), transform.parent)
            .GetComponent<SavedEntry>();
        obj.transform.position = pos;
        obj.Animate(PlayerInfo.Pass);
    }

    private void SpawnSprout(Vector3 pos)
    {
        var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/your_sprout"), transform.parent)
            .GetComponent<SavedEntry>();
        obj.transform.position = pos;
        obj.treeHeight = treeHeight;
        obj.Animate(PlayerInfo.Pass);
        spawned.Add(obj);
    }

    private void LoadLevel()
    {
        foreach (Transform platform in levelContainer.transform)
        {
            Destroy(platform.transform.GetChild(0).gameObject);
            platform.GetComponent<SavedEntry>().Animate(PlayerInfo.Pass);
        }

        foreach (Transform unliving in toLearnContainer.transform)
        {
            if (PlayerInfo.Learned.Contains(unliving.GetComponent<Unliving>().body.envType))
                Destroy(unliving.gameObject);
            else
                unliving.GetComponent<Unliving>().body.Animate(PlayerInfo.Pass);
        }
    }

    public void AddTerrains(List<Vector2> path, LineRenderer lineRenderer)
    {
        lineRenderer.transform.SetParent(transform, true);
        for (var i = 0; i < path.Count - 1; i++)
        {
            var center = (path[i] + path[i + 1]) / 2;
            var delta = path[i + 1] - path[i];
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, lineRenderer.transform);
            terr.transform.position = center;
            terr.transform.localScale = new Vector3(len, PlayerInfo.LineWidth, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x) / Mathf.PI * 180f);
            terr.GetComponent<SpriteRenderer>().color = Color.black;
        }

        Destroy(lineRenderer);
    }

    private void AddSavedTerrains(List<Vector3> path)
    {
        for (var i = 0; i < path.Count - 1; i++)
        {
            var center = (path[i] + path[i + 1]) / 2;
            var delta = path[i + 1] - path[i];
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, levelContainer.transform);
            terr.transform.position = center;
            terr.transform.localScale = new Vector3(len, PlayerInfo.LineWidth, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x) / Mathf.PI * 180f);
            terr.GetComponent<SpriteRenderer>().color = Color.black;
        }
    }

    private void Update()
    {
        if (IsPaused || PlayerInfo.Learned.Count == 0)
            return;
        foreach (var obj in spawned.Where(o => o.Clicked()))
        {
            if (obj.envType == "sprout")
            {
                Clear(obj.transform);
                var crone = Instantiate(Resources.Load<GameObject>("Prefabs/crone"), obj.transform);
                crone.transform.localScale = 2 * Vector3.one;

                crone.GetComponent<SavedEntry>().Animate(() => obj.Grow());
            }
        }

        PlayerInfo.UpdateIndex((int) Input.mouseScrollDelta.y);
        currentSpawn.text = PlayerInfo.CurrentType;
    }

    public static void Clear(Transform t)
    {
        foreach (Transform child in t)
            Destroy(child.gameObject);
    }

    public static void Clear(GameObject g) => Clear(g.transform);
}