using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DevDraw : SelfDraw
{
    public SavedEntry container;
    public TMP_InputField drawingType;
    public TMP_InputField title;
    public Toggle isTough;

    public void Save()
    {
        PrefabUtility.SaveAsPrefabAsset(container.gameObject, $"Assets/Prefabs/{title.text}.prefab");
        Game.Clear(container.transform);
        container.Init();
    }

    private void Start()
    {
        container.Init();
    }

    private void Update() => Render(Confirm);

    private void Confirm()
    {
        if (currentLineRenderer.positionCount < 2)
            return;
        currentLineRenderer.transform.SetParent(transform);
        
        var path = 
            Enumerable
                .Range(0, currentLineRenderer.positionCount)
                .Select(currentLineRenderer.GetPosition)
                .ToList();
        var path2 = path.Select(v => new Vector3(v.x, v.y)).ToList();
        container.Add(path2.ToStrList(), drawingType.text.ToLower(), isTough.isOn);
        currentLineRenderer = null;
    }
}