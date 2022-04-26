using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Game : MonoBehaviour
{
    private readonly List<Polyline> _terrain = new List<Polyline>();
    public Polyline CurrentPlatform;
    private void Start()
    {
        //AddTerrains(new List<Vector2>(){new Vector2(-5, -6), new Vector2(5, 6)});
    }

    public void AddTerrains(List<Vector2> path)
    {
        if (path.Count < 2)
            throw new ArgumentException();
        var polyline = new Polyline();
        var delta = path[1].x - path[0].x;
        polyline.Add(path[0]);
        print(path[0]);
        print(path[1]);
        for (var i = 1; i < path.Count; i++)
        {
            var dx = path[i].x - path[i - 1].x;
            if (delta * dx <= 0)
            {
                delta = dx;
                _terrain.Add(polyline);
                polyline = new Polyline();
            }

            polyline.Add(path[i]);
        }
        
        if(polyline.Count>1)
            _terrain.Add(polyline);

        //var player = FindObjectOfType<Player>();
        foreach(var p in _terrain)
        {
            var sb = new StringBuilder();
            foreach (var l in p.GetLines)
                sb.Append($"p1: {l.P1}, p2: {l.P2}\n");
            print(sb.ToString());
        }
    }

    public bool IsPlayerGrounded(Player p)
    {
        foreach (var polyline in _terrain)
        {
            if (polyline.IsAboveThePoint(p.FeetLow) && !polyline.IsAboveThePoint(p.FeetHigh))
            {
                CurrentPlatform = polyline;
                return true;
            }
        }

        CurrentPlatform = null;
        return false;
    }
    
    
}