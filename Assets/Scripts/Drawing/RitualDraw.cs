using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class RitualDraw : SelfDraw
{
    public SavedEntry container;
    public SavedEntry original;
    public string material;
    public string title;
    public bool isTough;
    public bool isSync;

    private void Start()
    {
        maxPoints = original.totalPoints * 1.5f;
        container.Init();
    }
    
    public void Save()
    {
        if (!Check())
        {
            Debug.Log("Try again!");
            return;
        }
        PrefabUtility.SaveAsPrefabAsset(container.gameObject, $"Assets/Resources/Prefabs/your_{title}.prefab");
        Game.Clear(container.transform);
        container.Init();
    }
    
    public void Clear()
    {
        container.transform.SetParent(transform.parent);
        Game.Clear(transform);
        Game.Clear(container.transform);
        container.Init();
        container.transform.SetParent(transform);
    }

    public bool Check()
    {
        var newLines = container.Lines.Select(line => line.ToVectList());
        foreach (var line in original.Lines)
        {
            var realLine = line.ToVectList();
            var keyPoints = realLine.KeyPoints();
            foreach (var keyPoint in keyPoints)
            {
                var check = false;
                foreach (var newLine in newLines)
                {
                    foreach (var newPoint in newLine)
                    {
                        if (Vector3.Distance(newPoint, keyPoint) > 0.5f)
                            continue;
                        check = true;
                        break;

                    }
                    if (check) break;
                }
            }
        }
        return true;
    }
    private void Update() => Render(Confirm);

    private void Confirm()
    {
        if (currentLineRenderer.positionCount < 2 || totalPoints > maxPoints)
        {
            Destroy(currentLineRenderer.gameObject);
            return;
        }
        currentLineRenderer.transform.SetParent(transform);
        
        var path = 
            Enumerable
                .Range(0, currentLineRenderer.positionCount)
                .Select(currentLineRenderer.GetPosition)
                .ToList();
        var path2 = path.Select(v => new Vector3(v.x, v.y)).ToList();
        container.Add(path2.ToStrList(), material, isTough, isSync);
        currentLineRenderer = null;
    }
}