using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SavedEntry : MonoBehaviour
{
    [SerializeField]public List<string> Lines;
    [SerializeField]public List<string> Materials;
    [SerializeField]public List<bool> Toughness;
    public GameObject terrainPref;
    public GameObject brush;
    public Material blood, earth, wood;
    private List<LineRenderer> Renderers;
    public void Start()
    {
        Renderers = new List<LineRenderer>();
        if (Lines == null) 
            return;
        print("kekus");
        StartCoroutine(DrawLines());

    }

    private IEnumerator DrawLines()
    {
        while (Lines.Count > 0)
        {
            var currLine = Lines[0];
            var currMat = Materials[0];
            var currTough = Toughness[0];
            yield return AddLine(currLine, currMat, currTough);
            Lines.RemoveAt(0);
            Materials.RemoveAt(0);
            Toughness.RemoveAt(0);
        }
    }

    public void Init()
    {
        Lines = new List<string>();
        Materials = new List<string>();
        Toughness = new List<bool>();
    }

    public void Add(string line, string mat, bool tough)
    {
        Lines.Add(line);
        Materials.Add(mat);
        Toughness.Add(tough);
    }

    private IEnumerator AddLine(string line, string mat, bool tough)
    {
        // var bruh = Instantiate(brush, transform);
        // var lineRenderer = bruh.GetComponent<LineRenderer>();
        // lineRenderer.material = null;
        // lineRenderer.positionCount = 0;
        var realLine = line.ToVectList();
        for (var i = 0; i < realLine.Count - 1; i++) {
            var curr = realLine[i];
            //lineRenderer.AddPoint(curr);
            if (!tough)
                yield break;
            var next = realLine[i + 1];
            var delta = next - curr;
            var center = curr + delta / 2;
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, transform);
            terr.transform.position = center;
            terr.transform.localScale = new Vector3(len, 0.2f, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x)/Mathf.PI*180f);
            terr.GetComponent<SpriteRenderer>().material = mat.ToLower() switch
            {
                "wood" => wood,
                "earth" => earth,
                "blood" => blood,
                _ => default
            };
            yield return new WaitForSeconds(Time.deltaTime);
        }
        // lineRenderer.AddPoint(realLine.Last());
        // Renderers.Add(lineRenderer);

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

    public static string ToStrList(this List<Vector3> source)
    {
        return string.Join("|", source.Select(ToCompactString));
    }
    public static List<Vector3> ToVectList(this string s)
    {
        return s.Split('|').Select(ToVector).ToList();
    }

    private static string ToCompactString(this Vector3 v)
    {
        return $"{v.x}_{v.y}_{v.z}";
    }

    private static Vector3 ToVector(this string s)
    {
        var split = s.Split('_');
        return new Vector3(float.Parse(split[0]), float.Parse(split[1]), float.Parse(split[2]));
    }
}