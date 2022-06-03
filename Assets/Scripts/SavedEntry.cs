using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedEntry : MonoBehaviour
{
    public Queue<List<Vector3>> Lines;
    public Queue<string> Materials;
    public Queue<bool> Toughness;
    private List<LineRenderer> Renderers;
    public void Start()
    {
        while (Lines.Count > 0)
        {
            var currLine = Lines.Dequeue();
            var currMat = Materials.Dequeue();
            var currTough = Toughness.Dequeue();
            StartCoroutine(AddLine(currLine, currMat, currTough));
        }
    }

    private IEnumerator AddLine(List<Vector3> line, string mat, bool tough)
    {
        var renderer = new LineRenderer();
        for (int i = 0; i < line.Count - 1; i++) {
            var curr = line[i];
            var next = line[i + 1];
            var delta = next - curr;
            
        }

        yield return null;
    }
    
}

public static class UnityExtensions
{
    public static void AddPoint(this LineRenderer rend, Vector3 pos)
    {
        var positionCount = rend.positionCount;
        var count = positionCount;
        positionCount++;
        rend.positionCount = positionCount;
        rend.SetPosition(count, pos);
    }
}