using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject terrainPref;
    private List<LineRenderer> Renderers;
    public static bool IsPaused;
    private void Start()
    {
        IsPaused = false;
        Renderers = new List<LineRenderer>();
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
    }

    private void Update()
    {
        if (IsPaused)
            return;
        foreach (var rend in Renderers)
        {
            for (var i = 0; i < rend.positionCount; i++)
            {
                var delta = new Vector2();
                float dx;
                float dy;
                var t = Time.time+i/10f;
                var dt = t - Time.deltaTime;
                switch (i % 4)
                {
                    case 0:
                        dx = Mathf.Cos(t)-Mathf.Cos(dt);
                        dy = Leaf(Mathf.Cos(t))-Leaf(Mathf.Cos(dt));
                        delta = new Vector2(dx, dy);
                        break;
                    case 1:
                        dy = Mathf.Cos(t)-Mathf.Cos(dt);
                        dx = Leaf(Mathf.Cos(t))-Leaf(Mathf.Cos(dt));
                        delta = new Vector2(dx, dy);
                        break;
                    case 2:
                        dx = Mathf.Cos(t)-Mathf.Cos(dt);
                        dy = Leaf(Mathf.Cos(t))-Leaf(Mathf.Cos(dt));
                        delta = new Vector2(dx, -dy);
                        break;
                    case 3:
                        dy = Mathf.Cos(t)-Mathf.Cos(dt);
                        dx = Leaf(Mathf.Cos(t))-Leaf(Mathf.Cos(dt));
                        delta = new Vector2(dx, -dy);
                        break;
                }
                rend.SetPosition(i, rend.GetPosition(i)+(Vector3)delta/6);
            }
        }
    }

    private float Leaf(float arg) => Mathf.Sqrt(Mathf.Pow(arg, 4) - Mathf.Pow(arg, 6));
}