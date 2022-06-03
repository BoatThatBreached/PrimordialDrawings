using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SavedEntry : MonoBehaviour
{
    [SerializeField]public List<string> lines;
    [SerializeField]public List<string> materials;
    [SerializeField]public List<bool> toughness;
    public GameObject terrainPref;
    public Material blood, earth, wood;
    public bool synchronized;

    public string envType;
    
    public void Animate()
    {
        StartCoroutine(DrawLines());
    }

    private IEnumerator DrawLines()
    {
        for(var i = 0; i<lines.Count; i++)
        {
            if (synchronized)
                StartCoroutine(AddLine(lines[i], materials[i], toughness[i]));
            else
                yield return AddLine(lines[i], materials[i], toughness[i]);
        }
    }

    public Action Click;

    public void OnMouseUpAsButton() => Click();

    public void Init()
    {
        lines = new List<string>();
        materials = new List<string>();
        toughness = new List<bool>();
    }

    public void Add(string line, string mat, bool tough, bool sync)
    {
        lines.Add(line);
        materials.Add(mat);
        toughness.Add(tough);
        synchronized = sync;
    }

    private IEnumerator AddLine(string line, string mat, bool tough)
    {
        var realLine = line.ToVectList();
        for (var i = 0; i < realLine.Count - 1; i++) {
            var curr = realLine[i];
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