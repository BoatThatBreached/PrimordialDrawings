using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public Camera mCamera;
    public GameObject brush;
    private Vector2 _lastPos;
    private LineRenderer _currentLineRenderer;
    public Game game;
    public Material terrainMaterial;

    private void Start()
    {
        CreateBrush();
        var path2 = Enumerable.Range(-50, 101).Select(i => new Vector2(i / 5f, -3 + Mathf.Sin(i/12f))).ToList();
        _currentLineRenderer.positionCount = 0;
        foreach(var p in path2)
            AddPoint(p);
        _currentLineRenderer.material = terrainMaterial;
        game.AddTerrains(path2.ToList(), _currentLineRenderer.gameObject);
        _currentLineRenderer = null;
    }

    private void Update() => Render();
    
    private void Render()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateBrush();
        }

        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = mCamera.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos != _lastPos)
            {
                AddPoint(mousePos);
                _lastPos = mousePos;
            }
        }
        else
        {

            if (_currentLineRenderer)
            {
                _currentLineRenderer.material = terrainMaterial;
                var path = new List<Vector2>(){_currentLineRenderer.GetPosition(0)};
                var count = _currentLineRenderer.positionCount;
                for (var i = 1; i < count - 1; i++)
                {
                    var prev = path.Last();
                    var curr = _currentLineRenderer.GetPosition(i);
                    var curr2 = new Vector2(curr.x, curr.y);
                    var d1 = curr2 - prev;
                    if (d1.sqrMagnitude < 0.2f)
                        continue;
                    path.Add(curr2);
                }
                path.Add(_currentLineRenderer.GetPosition(count-1));
                print($"{count} -> {path.Count}");
                var path2 = path.Select(v => new Vector2(v.x, v.y)).ToList();
                

                _currentLineRenderer.positionCount = 0;
                foreach(var p in path2)
                    AddPoint(p);
                game.AddTerrains(path2.ToList(), _currentLineRenderer.gameObject);
            } 
            _currentLineRenderer = null;
        }
    }

    private void CreateBrush()
    {
        var bruh = Instantiate(brush);
        _currentLineRenderer = bruh.GetComponent<LineRenderer>();
        var mousePos = mCamera.ScreenToWorldPoint(Input.mousePosition);
        _currentLineRenderer.SetPosition(0, mousePos);
        _currentLineRenderer.SetPosition(1, mousePos);
    }

    private void AddPoint(Vector2 pos)
    {
        var positionCount = _currentLineRenderer.positionCount;
        var count = positionCount;
        positionCount++;
        _currentLineRenderer.positionCount = positionCount;
        _currentLineRenderer.SetPosition(count, pos);
    }
}