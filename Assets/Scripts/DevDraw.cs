using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DevDraw : SelfDraw
{
    public GameObject container;
    private List<GameObject> drawingQueue;
    public TMP_InputField drawingType;
    public TMP_InputField title;
    public Material blood, earth, wood;
    public Toggle isTough;
    //public List<LineRenderer> Renderers;
    //public List<Vector3> Centers;
    public GameObject terrainPref;

    public void Save()
    {
        
        PrefabUtility.SaveAsPrefabAsset(gameObject, $"Assets/Prefabs/{title.text}.prefab");
    }

    private void Start()
    {
        drawingQueue = new List<GameObject>();
    }

    private void Update() => Render(Confirm);

    private void Confirm()
    {
        if (currentLineRenderer.positionCount < 2)
            return;
        currentLineRenderer.transform.SetParent(container.transform);
        currentLineRenderer.material = drawingType.text.ToLower() switch
        {
            "wood" => wood,
            "blood" => blood,
            "earth" => earth,
            _ => throw new ArgumentOutOfRangeException()
        };

        var path = 
            Enumerable
                .Range(0, currentLineRenderer.positionCount)
                .Select(currentLineRenderer.GetPosition)
                .ToList();
        var path2 = path.Select(v => new Vector2(v.x, v.y)).ToList();
        AddTerrains(path2.ToList(), currentLineRenderer, isTough.isOn);
        currentLineRenderer = null;
    }
    
    public void AddTerrains(List<Vector2> path, LineRenderer lineRenderer, bool addCollider)
    {
        lineRenderer.transform.SetParent(transform, true);
        //Renderers.Add(lineRenderer);
        //Centers.Add(path.Aggregate((p1, p2)=>p1+p2)/path.Count);
        drawingQueue.Add(lineRenderer.gameObject);
        if (!addCollider)
            return;
        for (var i = 0; i < path.Count - 1; i++)
        {
            var center = (path[i] + path[i + 1]) / 2;
            var delta = path[i + 1] - path[i];
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, transform);
            terr.transform.position = center;
            terr.transform.localScale = new Vector3(len, 0.2f, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x)/Mathf.PI*180f);
        }
        
    }
}