using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelfDraw: MonoBehaviour
{
    public GameObject brush;
    public LineRenderer currentLineRenderer;

    public void CreateBrush()
    {
        var bruh = Instantiate(brush);
        currentLineRenderer = bruh.GetComponent<LineRenderer>();
        currentLineRenderer.positionCount = 0;
    }

    public void AddPoint(Vector2 pos)
    {
        var positionCount = currentLineRenderer.positionCount;
        var count = positionCount;
        positionCount++;
        currentLineRenderer.positionCount = positionCount;
        currentLineRenderer.SetPosition(count, pos);
    }
}