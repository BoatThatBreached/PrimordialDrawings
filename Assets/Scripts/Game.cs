using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject terrainPref;
    private List<LineRenderer> Renderers;
    private List<Vector3> Centers;
    public static bool IsPaused;
    private void Start()
    {
        IsPaused = false;
        Renderers = new List<LineRenderer>();
        Centers = new List<Vector3>();
    }

    public void AddTerrains(List<Vector2> path, GameObject renderer)
    {
        Renderers.Add(renderer.GetComponent<LineRenderer>());
        if (path.Count < 2)
            throw new ArgumentException();
        for (var i = 0; i < path.Count - 1; i++)
        {
            var center = (path[i] + path[i + 1]) / 2;
            var delta = path[i + 1] - path[i];
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, transform);
            terr.transform.position = center;
            terr.transform.localScale = new Vector3(len, 0.2f, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x)/Mathf.PI*180f);
        }
        Centers.Add(path.Aggregate((p1, p2)=>p1+p2)/path.Count);
    }

    private void Update()
    {
        if (IsPaused)
            return;
        // foreach (var rend in Renderers)
        // {
        //     for (var i = 0; i < rend.positionCount; i++)
        //     {
        //         var delta = new Vector2();
        //         float dx;
        //         float dy;
        //         var t = Time.time+i/10f;
        //         var dt = t - Time.deltaTime;
        //         switch (i % 4)
        //         {
        //             case 0:
        //                 dx = Mathf.Cos(t)-Mathf.Cos(dt);
        //                 dy = Leaf(Mathf.Cos(t))-Leaf(Mathf.Cos(dt));
        //                 delta = new Vector2(dx, dy);
        //                 break;
        //             case 1:
        //                 dy = Mathf.Cos(t)-Mathf.Cos(dt);
        //                 dx = Leaf(Mathf.Cos(t))-Leaf(Mathf.Cos(dt));
        //                 delta = new Vector2(dx, dy);
        //                 break;
        //             case 2:
        //                 dx = Mathf.Cos(t)-Mathf.Cos(dt);
        //                 dy = Leaf(Mathf.Cos(t))-Leaf(Mathf.Cos(dt));
        //                 delta = new Vector2(dx, -dy);
        //                 break;
        //             case 3:
        //                 dy = Mathf.Cos(t)-Mathf.Cos(dt);
        //                 dx = Leaf(Mathf.Cos(t))-Leaf(Mathf.Cos(dt));
        //                 delta = new Vector2(dx, -dy);
        //                 break;
        //         }
        //         rend.SetPosition(i, rend.GetPosition(i)+(Vector3)delta/6);
        //     }
        // }

        for (var i = 0; i < Renderers.Count; i++)
        {
            var c = Centers[i];
            var t = Time.time;
            for (var j = 0; j < Renderers[i].positionCount; j++)
            {
                var delta = Renderers[i].GetPosition(j) - c;
                delta *= 1 + (Leaf(Mathf.Cos(t+j/20)) - Leaf(Mathf.Cos(t - Time.deltaTime+j/20)))/10;
                Renderers[i].SetPosition(j, c+delta);
            }
        }
    }

    private float Leaf(float arg) => Mathf.Sqrt(Mathf.Pow(arg, 4) - Mathf.Pow(arg, 6));
}