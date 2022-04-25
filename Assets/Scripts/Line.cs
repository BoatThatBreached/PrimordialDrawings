using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line
{
    private double Slope;
    private double Offset;
    private bool isOrtho;
    public Vector2 P1;
    public Vector2 P2;
    public double Evaluate(double x) => isOrtho ? double.PositiveInfinity : Slope * x + Offset;


    public Line(Vector2 p1, Vector2 p2)
    {
        var dx = p2.x - p1.x;
        var dy = p2.y - p1.y;
        isOrtho = Mathf.Abs(dx) < 1e-7;
        Slope = isOrtho ? 0 : dy / dx;
        Offset = isOrtho ? 0 : p1.y - p1.x * Slope;
        P1 = p1;
        P2 = p2;
    }

    public float LeftBound() => Mathf.Min(P1.x, P2.x);
    public float RightBound() => Mathf.Max(P1.x, P2.x);
}

public class Polyline
{
    private List<Line> Lines;

    public Polyline()
    {
        Lines = new List<Line>();
    }

    public bool IsAbove(Vector2 pos)
    {
        foreach (var line in Lines)
            if (pos.x >= line.LeftBound() && pos.x <= line.RightBound() && line.Evaluate(pos.x) < pos.y)
                return false;
        return true;
    }

    public bool IsUnder(Vector2 pos)
    {
        foreach (var line in Lines)
            if (pos.x >= line.LeftBound() && pos.x <= line.RightBound() && line.Evaluate(pos.x) > pos.y)
                return false;
        return true;
    }

    public void Add(Line l) => Lines.Add(l);
    public int Count => Lines.Count;
}