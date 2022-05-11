using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MouseDraw : MonoBehaviour
{
    public Camera mCamera;
    public GameObject brush;
    private Vector2 _lastPos;
    public LineRenderer currentLineRenderer;
    public Game game;
    public Material terrainMaterial;

    private void Start()
    {
        // CreateBrush();
        // var path2 = Enumerable.Range(-50, 101).Select(i => new Vector2(i / 5f, -3 + Mathf.Sin(i / 12f))).ToList();
        // currentLineRenderer.positionCount = 0;
        // foreach (var p in path2)
        //     AddPoint(p);
        // currentLineRenderer.material = terrainMaterial;
        // game.AddTerrains(path2.ToList(), currentLineRenderer);
        // currentLineRenderer = null;
    }

    private void Update() => Render();

    private void Render()
    {
        if (Input.GetMouseButtonDown(0))
            CreateBrush();
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = mCamera.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos == _lastPos)
                return;
            AddPoint(mousePos);
            _lastPos = mousePos;
        }
        else if (currentLineRenderer)
            Flush();
    }

    public void Flush()
    {
        currentLineRenderer.material = terrainMaterial;
        var path = new List<Vector2>() {currentLineRenderer.GetPosition(0)};
        var count = currentLineRenderer.positionCount;
        for (var i = 1; i < count - 1; i++)
        {
            var prev = path.Last();
            var curr = currentLineRenderer.GetPosition(i);
            var curr2 = new Vector2(curr.x, curr.y);
            var d1 = curr2 - prev;
            if (d1.sqrMagnitude < 0.2f)
                continue;
            path.Add(curr2);
        }

        path.Add(currentLineRenderer.GetPosition(count - 1));
        print($"{count} -> {path.Count}");
        var path2 = path.Select(v => new Vector2(v.x, v.y)).ToList();


        currentLineRenderer.positionCount = 0;
        foreach (var p in path2)
            AddPoint(p);
        game.AddTerrains(path2.ToList(), currentLineRenderer);
        currentLineRenderer = null;
    }

    public void CreateBrush()
    {
        var bruh = Instantiate(brush);
        currentLineRenderer = bruh.GetComponent<LineRenderer>();
        var mousePos = mCamera.ScreenToWorldPoint(Input.mousePosition);
        currentLineRenderer.SetPosition(0, mousePos);
        currentLineRenderer.SetPosition(1, mousePos);
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