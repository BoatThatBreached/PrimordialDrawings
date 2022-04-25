using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    private readonly List<Polyline> _terrain = new List<Polyline>();

    private void Start()
    {
        //AddTerrains(new List<Vector2>(){new Vector2(-5, -6), new Vector2(5, 6)});
    }

    public void AddTerrains(List<Vector2> path)
    {
        if (path.Count < 2)
            throw new ArgumentException();
        var polyline = new Polyline();
        polyline.Add(new Line(path[0], path[1]));
        
        var delta = path[1].x - path[0].x;
        for (var i = 2; i < path.Count; i++)
        {
            var dx = path[i].x - path[i - 1].x;
            if (delta * dx <= 0)
            {
                delta = dx;
                _terrain.Add(polyline);
                polyline = new Polyline();
            }

            polyline.Add(new Line(path[i-1], path[i]));
        }
        
        _terrain.Add(polyline);
        
        
        foreach(var p in _terrain)
            print(p.Count);
    }

    public bool IsPlayerGrounded(Player p)
    {
        var feet = p.Feet;
        foreach (var polyline in _terrain)
        {
            if (polyline.IsUnder(feet+new Vector2(0, 0.02f)) && polyline.IsAbove(feet))
                return true;
        }

        return false;
    }
}