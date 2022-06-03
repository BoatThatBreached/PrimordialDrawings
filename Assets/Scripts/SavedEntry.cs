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
    public Material blood, earth, wood;
    public bool synchronized;

    public string envType;
    //private List<LineRenderer> Renderers;
    public void Animate()
    {
        //Renderers = new List<LineRenderer>();
        if (Lines == null) 
            return;
        print("kekus");
        StartCoroutine(DrawLines());

    }

    private IEnumerator DrawLines()
    {
        for(var i = 0; i<Lines.Count; i++)
        {
            if (synchronized)
                StartCoroutine(AddLine(Lines[i], Materials[i], Toughness[i]));
            else
                yield return AddLine(Lines[i], Materials[i], Toughness[i]);
        }
        
        yield return 1;
    }

    public Action Click;

    public void OnMouseUpAsButton() => Click();

    public void Init()
    {
        Lines = new List<string>();
        Materials = new List<string>();
        Toughness = new List<bool>();
    }

    public void Add(string line, string mat, bool tough, bool sync)
    {
        Lines.Add(line);
        Materials.Add(mat);
        Toughness.Add(tough);
        synchronized = sync;
    }

    private IEnumerator AddLine(string line, string mat, bool tough)
    {
        var realLine = line.ToVectList();
        for (var i = 0; i < realLine.Count - 1; i++) {
            var curr = realLine[i];
            Debug.Log(curr.x + " " + curr.y);
            var next = realLine[i + 1];
            var delta = next - curr;
            var center = curr + delta / 2;
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, transform);
            terr.transform.position = center+transform.position;
            terr.transform.localScale = new Vector3(len, 0.2f, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x)/Mathf.PI*180f);
            if(!tough)
                Destroy(terr.GetComponent<BoxCollider2D>());
            terr.GetComponent<SpriteRenderer>().material = mat.ToLower() switch
            {
                "1" => wood,
                "2" => earth,
                "0" => blood,
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

    public static List<Vector3> KeyPoints(this List<Vector3> line)
    {
        var keyPoints = new List<Vector3> { line[0] };
        var prev = line[0];
        for (var i = 1; i < line.Count - 1; i++)
        {
            var point = line[i];
            var next = line[i + 1];
            if (Vector3.Distance(prev, point) >= 0.2f &&
                Math.Cos(Vector3.Angle(point - prev, next - point)) >= -Math.Sqrt(2) / 2)
                keyPoints.Add(point);
        }
        keyPoints.Add(line[line.Count - 1]);
        return keyPoints;
    }
}