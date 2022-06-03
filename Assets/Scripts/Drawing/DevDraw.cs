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
    public TMP_Dropdown drawingType;
    public TMP_InputField title;
    public Toggle isTough;
    public Toggle isSync;

    public void Save()
    {
        PrefabUtility.SaveAsPrefabAsset(container.gameObject, $"Assets/Resources/Prefabs/{title.text}.prefab");
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

    private void Start()
    {
        container.Init();
    }

    private void Update() => Render(Confirm);

    private void Confirm()
    {
        if (currentLineRenderer.positionCount < 2)
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
        container.Add(path2.ToStrList(), drawingType.value.ToString(), isTough.isOn, isSync.isOn);
        currentLineRenderer = null;
    }
}