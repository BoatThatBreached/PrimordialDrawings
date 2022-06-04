using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public GameObject terrainPref;

    public TMP_Text currentSpawn;
    // private List<LineRenderer> Renderers;
    // private List<Vector3> Centers;
    public static bool IsPaused;
    private void Awake()
    {
        PlayerInfo.Reset();
        IsPaused = false;
        //Renderers = new List<LineRenderer>();
        //Centers = new List<Vector3>();
    }

    public void AddTerrains(List<Vector2> path, LineRenderer lineRenderer, bool addCollider=true)
    {
        lineRenderer.transform.SetParent(transform, true);
        // Renderers.Add(lineRenderer);
        // Centers.Add(path.Aggregate((p1, p2)=>p1+p2)/path.Count);
        if (!addCollider)
            return;
        for (var i = 0; i < path.Count - 1; i++)
        {
            var center = (path[i] + path[i + 1]) / 2;
            var delta = path[i + 1] - path[i];
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, lineRenderer.transform);
            terr.transform.position = center;
            terr.transform.localScale = new Vector3(len, 0.2f, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x)/Mathf.PI*180f);
            terr.GetComponent<SpriteRenderer>().color = Color.black;
        }
        Destroy(lineRenderer);
    }

    private void FixedUpdate()
    {
        if (IsPaused||PlayerInfo.Learned.Count==0)
            return;
        
        PlayerInfo.UpdateIndex((int) Input.mouseScrollDelta.y);
        currentSpawn.text = PlayerInfo.CurrentType;

        //
        // for (var i = 0; i < Renderers.Count; i++)
        // {
        //     var c = Centers[i];
        //     var t = Time.time;
        //     var a = 20f;
        //     var b = 10f;
        //     for (var j = 0; j < Renderers[i].positionCount; j++)
        //     {
        //         var delta = Renderers[i].GetPosition(j) - c;
        //         delta *= 1 + (Leaf(Mathf.Cos(t+j/a)) - Leaf(Mathf.Cos(t - Time.fixedDeltaTime+j/a)))/b;
        //         Renderers[i].SetPosition(j, c+delta);
        //     }
        // }
    }

    //public static float Leaf(float arg) => Mathf.Sqrt(Mathf.Pow(arg, 4) - Mathf.Pow(arg, 6));

    public static void Clear(Transform t)
    {
        foreach(Transform child in t)
            Destroy(child.gameObject);
    }

    public static void Clear(GameObject g) => Clear(g.transform);
}