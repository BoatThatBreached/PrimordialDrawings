using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelfDraw: MonoBehaviour
{
    public Camera mCamera;
    public GameObject brush;
    public LineRenderer currentLineRenderer;
    public Vector2 lastPos;
    public float totalPoints;
    public float maxPoints = 100000;

    public void CreateBrush()
    {
        var bruh = Instantiate(brush, transform);
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
    
    public void Render(Action flush)
    {
        if (Input.GetMouseButtonDown(0))
            CreateBrush();
        else if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = mCamera.ScreenToWorldPoint(Input.mousePosition);
            var len = (mousePos - lastPos).magnitude;
            if (len < 0.2f)
                return;
            totalPoints += 0.1f;
            if (totalPoints > maxPoints)
            {
                totalPoints = maxPoints;
                return;
            }

            AddPoint(mousePos);
            lastPos = mousePos;
        }
        else if (currentLineRenderer)
        {
            
            flush();
        }
    }
}