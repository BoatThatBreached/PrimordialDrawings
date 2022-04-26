using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line
{
    private float Slope;
    private float Offset;
    private bool isOrtho;
    public Vector2 P1;
    public Vector2 P2;
    public float Evaluate(float x) => isOrtho ? float.PositiveInfinity : Slope * x + Offset;
    public override string ToString()
    {
        return $"slope: {Slope}, offset: {Offset}";
    }

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
    private Vector2 last;
    private bool started;
    public float rightBound;
    public float leftBound;

    public Polyline()
    {
        Lines = new List<Line>();
        started = false;
    }

    public float Evaluate(Vector2 pos)
    {
        var line = Lines.FirstOrDefault(line => line.LeftBound() < pos.x && line.RightBound() > pos.x);
        return line?.Evaluate(pos.x) ?? pos.y;
    }

    public float DiffY(Vector2 pos) => pos.y - Evaluate(pos);

    public bool IsAboveThePoint(Vector2 pos)
    {
        if (pos.x < leftBound || pos.x > rightBound)
            return false;
        foreach (var line in Lines)
        {
            if (pos.x < line.LeftBound() || pos.x > line.RightBound())
                continue;
            if (line.Evaluate(pos.x) < pos.y)
                return false;
        }

        return true;
    }

    public void Add(Line l) => Lines.Add(l);

    public void Add(Vector2 pos)
    {
        if (started)
        {
            Lines.Add(new Line(last, pos));
            leftBound = Mathf.Min(leftBound, pos.x);
            rightBound = Mathf.Max(rightBound, pos.x);
        }

        last = pos;
        started = true;
    }

    public int Count => Lines.Count;

    public IEnumerable<Line> GetLines
    {
        get
        {
            foreach (var line in Lines)
                yield return line;
        }
    }
}
