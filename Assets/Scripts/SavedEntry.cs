using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using Game_Objects;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SavedEntry : MonoBehaviour
{
    [SerializeField] [FormerlySerializedAs("Lines")]
    public List<string> lines;

    [SerializeField] [FormerlySerializedAs("Materials")]
    public List<string> materials;

    [SerializeField] [FormerlySerializedAs("Toughness")]
    public List<bool> toughness;

    public GameObject terrainPref;
    private int _totalPoints;
    private int _drawnPoints;
    public bool synchronized;

    public string envType;
    private float _minX, _minY, _maxX, _maxY;
    private bool _moving;
    private float _direction;

    public float Volume => 1 - (float) _drawnPoints / _totalPoints;
    //private Vector3 _center;

    private List<Transform> _embers;
    private List<Transform> _flames;

    public float treeHeight;

    public void Move(float dir)
    {
        _moving = true;
        _direction = dir;

        if (envType == "spear")
        {
            var tigers = FindObjectsOfType<Animal>();
            foreach (var t in tigers)
                if (Vector3.Distance(t.transform.position, transform.position) < 1f)
                {
                    Destroy(t.gameObject);
                    Destroy(gameObject);
                    return;
                }
        }
    }

    private void Update()
    {
        if (_moving)
        {
            transform.position += new Vector3(1, _direction, 0) * Time.deltaTime * 20;
        }
    }

    public void Start()
    {
        _embers = new List<Transform>();
        _flames = new List<Transform>();
        if (lines == null || lines.Count == 0)
            return;
        var points = lines
            .Select(line => line.ToVectList())
            .SelectMany(l => l)
            .Select(v => v.Extend(transform.localScale))
            .ToList();
        _minX = points.Min(v => v.x) + transform.position.x;
        _minY = points.Min(v => v.y) + transform.position.y;
        _maxX = points.Max(v => v.x) + transform.position.x;
        _maxY = points.Max(v => v.y) + transform.position.y;
    }

    public void Animate(Action finish)
    {
        if (lines == null || lines.Count == 0)
            return;
        //_center = 1f/allPoints.Count*allPoints.Aggregate((v1, v2) => v1 + v2);
        StartCoroutine(DrawLines(finish));
    }

    private IEnumerator DrawLines(Action finish)
    {
        _totalPoints = lines.SelectMany(line => line.ToVectList()).ToList().DividedPoints(0.3f).Count;
        _drawnPoints = 0;
        if (synchronized)
        {
            for (var i = 0; i < lines.Count; i++)
                StartCoroutine(AddLine(lines[i], materials[i], toughness[i]));
            yield return new WaitForSeconds(_totalPoints / lines.Count/30f);
        }
        else
            for (var i = 0; i < lines.Count; i++)
                yield return AddLine(lines[i], materials[i], toughness[i]);
        finish();
    }

    public Action Click;

    public void Init()
    {
        _totalPoints = 0;
        lines = new List<string>();
        materials = new List<string>();
        toughness = new List<bool>();
        _embers = new List<Transform>();
    }

    public void Add(string line, string mat, bool tough, bool sync)
    {
        lines.Add(line);
        materials.Add(mat);
        toughness.Add(tough);
        synchronized = sync;
    }

    private void AddLineMomentum(List<Vector3> line, string mat, bool tough)
    {
        var container = Instantiate(terrainPref, transform);
        container.transform.localScale = Vector3.one;
        _flames.Add(container.transform);
        Destroy(container.GetComponent<SpriteRenderer>());
        Destroy(container.GetComponent<BoxCollider2D>());
        var realLine = line
            .DividedPoints(0.3f)
            .Select(v => v.Extend(transform.localScale))
            .Select(v => v.Rotate(
                // _center,
                new Vector3(),
                transform.localEulerAngles.z / 180 * Mathf.PI))
            .ToList();
        container.transform.localPosition = new Vector3();
        //_totalPoints += realLine.Count;
        for (var i = 0; i < realLine.Count - 1; i++)
        {
            var curr = realLine[i];
            var next = realLine[i + 1];
            var delta = next - curr;
            var center = curr + delta / 2;
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, container.transform);
            terr.transform.position = center + transform.position;
            terr.transform.localScale = new Vector3(len, PlayerInfo.LineWidth, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x) / Mathf.PI * 180f)
                                              - transform.eulerAngles
                ;
            if (!tough)
                Destroy(terr.GetComponent<BoxCollider2D>());
            terr.GetComponent<SpriteRenderer>().color = mat.ToLower() switch
            {
                "0" => new Color(0.4078431f, 0, 0),
                "1" => new Color(1, 0.5714479f, 0),
                "2" => new Color(1f / 255, 1f / 255, 1f / 255),
                "3" => new Color(1f, 1f, 0),
                "4" => new Color(1f, 0f, 0),
                "5" => Color.white,
                "6" => Color.blue,
                _ => Color.white
            };
            _drawnPoints++;
        }

        _drawnPoints++;
    }

    private IEnumerator AddLine(string line, string mat, bool tough)
    {
        var container = Instantiate(terrainPref, transform);
        container.transform.localScale = Vector3.one;

        Destroy(container.GetComponent<SpriteRenderer>());
        Destroy(container.GetComponent<BoxCollider2D>());
        var realLine = line
            .ToVectList()
            .DividedPoints(0.3f)
            .Select(v => v.Extend(transform.localScale))
            .Select(v => v.Rotate(
                // _center,
                new Vector3(),
                transform.localEulerAngles.z / 180 * Mathf.PI))
            .ToList();
        container.transform.position = realLine.Center() + transform.position;
        //_totalPoints += realLine.Count;
        for (var i = 0; i < realLine.Count - 1; i++)
        {
            var curr = realLine[i];
            var next = realLine[i + 1];
            var delta = next - curr;
            var center = curr + delta / 2;
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, container.transform);
            terr.transform.position = center + transform.position;
            terr.transform.localScale = new Vector3(len, PlayerInfo.LineWidth, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x) / Mathf.PI * 180f)
                                              - transform.eulerAngles
                ;
            if (!tough)
                Destroy(terr.GetComponent<BoxCollider2D>());
            terr.GetComponent<SpriteRenderer>().color = mat.ToLower() switch
            {
                "0" => new Color(0.4078431f, 0, 0),
                "1" => new Color(1, 0.5714479f, 0),
                "2" => new Color(1f / 255, 1f / 255, 1f / 255),
                "3" => new Color(1f, 1f, 0),
                "4" => new Color(1f, 0f, 0),
                "5" => Color.white,
                _ => Color.white
            };
            yield return new WaitForSeconds(Time.deltaTime / 10);
            _drawnPoints++;
        }

        _drawnPoints++;
    }

    private IEnumerator AddLine(List<Vector3> line, string mat, bool tough)
    {
        var container = Instantiate(terrainPref, transform);
        container.transform.localScale = Vector3.one;

        Destroy(container.GetComponent<SpriteRenderer>());
        Destroy(container.GetComponent<BoxCollider2D>());
        var realLine = line
            .DividedPoints(0.3f)
            .Select(v => v.Extend(transform.localScale))
            .Select(v => v.Rotate(
                // _center,
                new Vector3(),
                transform.localEulerAngles.z / 180 * Mathf.PI))
            .ToList();
        container.transform.position = realLine.Center() + transform.position;
        //_totalPoints += realLine.Count;
        for (var i = 0; i < realLine.Count - 1; i++)
        {
            var curr = realLine[i];
            var next = realLine[i + 1];
            var delta = next - curr;
            var center = curr + delta / 2;
            var len = delta.magnitude;
            var terr = Instantiate(terrainPref, container.transform);
            terr.transform.position = center + transform.position;
            terr.transform.localScale = new Vector3(len, PlayerInfo.LineWidth, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x) / Mathf.PI * 180f)
                                              - transform.eulerAngles
                ;
            if (!tough)
                Destroy(terr.GetComponent<BoxCollider2D>());
            terr.GetComponent<SpriteRenderer>().color = mat.ToLower() switch
            {
                "0" => new Color(0.4078431f, 0, 0),
                "1" => new Color(1, 0.5714479f, 0),
                "2" => new Color(1f / 255, 1f / 255, 1f / 255),
                "3" => new Color(1f, 1f, 0),
                "4" => new Color(1f, 0f, 0),
                "5" => Color.white,
                _ => Color.white
            };
            yield return new WaitForSeconds(Time.deltaTime / 10);
            _drawnPoints++;
        }

        _drawnPoints++;
    }

    public bool Clicked()
    {
        if (Camera.main == null)
            return false;
        var mpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Input.GetMouseButtonDown(0) && _minX <= mpos.x && mpos.x <= _maxX && _minY <= mpos.y && mpos.y <= _maxY;
    }

    public void Burn()
    {
        foreach (var t in _flames)
            Destroy(t.gameObject);
        _flames.Clear();
        for (var i = 0; i < 5; i++)
        {
            var line =
                Enumerable
                    .Range(0, 10 - 2 * Math.Abs(2 - i))
                    .Skip(Math.Abs(2 - i))
                    .Select(y => y / 2.5f)
                    .ToList();
            var realLine = new List<Vector3> {new Vector3((i - 2f) / 1.4f, line[0] + 1, 15)};
            for (var j = 1; j < line.Count; j++)
                realLine.Add(new Vector3(Mathf.Cos(line[j] + Time.time) / 2 + (i - 2f) / 1.4f, line[j] + 1, 15));
            AddLineMomentum(realLine, i == 2 ? "1" : i % 4 == 0 ? "4" : "3", false);
        }
    }

    public void Ashen(int emberCount, float dt, float startHeight, float offset, float speed)
    {
        while (_embers.Count < emberCount)
        {
            var t = Instantiate(terrainPref, transform).transform;
            Destroy(t.GetComponent<BoxCollider2D>());
            t.GetComponent<SpriteRenderer>().color = new Color(7f / 255, 7f / 255, 7f / 255);
            t.position = new Vector3(Random.Range(_minX, _maxX),
                Random.Range(_minY / 2 + _maxY / 2, _maxY + offset + startHeight) + startHeight, -15);
            _embers.Add(t);
        }

        foreach (var t in _embers)
            t.position += dt * speed * (3 + Mathf.Sin(Time.time)) * Vector3.up;
        var toDestroy = _embers.Where(t => t.position.y >= _maxY + offset + startHeight).ToList();
        foreach (var t in toDestroy)
        {
            _embers.Remove(t);
            Destroy(t.gameObject);
        }

        //_embers = _embers.Where(t => t.position.y < maxY + 3).ToList();
    }

    public void Float(float len, float speed, float amplitude)
    {
        foreach (var t in _flames)
            Destroy(t.gameObject);
        _flames.Clear();
        const int n = 12;
        var realPoints = new List<Vector3>(); // {new Vector3(-len/2, amplitude*Mathf.Sin(-speed*len/2), -15)};
        realPoints.AddRange(
            Enumerable
                .Range(-n, 2 * n + 1)
                .Select(i => i * len / 2 / n)
                .Select(x => new Vector3(x, amplitude * Mathf.Sin(speed * (x + Time.time)), -15)));
        // realPoints.Add(new Vector3(len/2, amplitude*Mathf.Sin(speed*len/2), -15));
        AddLineMomentum(realPoints, "6", false);
    }

    public void EndAshen(float dt, float startHeight, float offset, float speed)
    {
        if (_embers.Count == 0)
            return;
        foreach (var t in _embers)
            t.position += dt * speed * (3 + Mathf.Sin(Time.time)) * Vector3.up;
        var toDestroy = _embers.Where(t => t.position.y >= _maxY + offset + startHeight).ToList();
        foreach (var t in toDestroy)
        {
            _embers.Remove(t);
            Destroy(t.gameObject);
        }
    }

    public void Grow()
    {
        if (envType == "sprout")
            StartCoroutine(GrowTree());
    }

    private IEnumerator GrowTree()
    {
        var height = 0f;
        var dh = 0.8f;
        var width = 1.2f;
        var dw = 0.2f;
        while (height < treeHeight)
        {
            var currX = Mathf.Sin(height);
            var upPointLeft = new Vector3(currX - width / 2, -height, -15);
            var upPointRight = new Vector3(currX + width / 2, -height, -15);
            height += dh;
            transform.position += dh * Vector3.up;
            width += dw;
            currX = Mathf.Sin(height);
            var downPointLeft = new Vector3(currX - width / 2, -height, -15);
            var downPointRight = new Vector3(currX + width / 2, -height, -15);
            yield return AddLine(new List<Vector3> {upPointLeft, downPointLeft, downPointRight, upPointRight}, "1",
                false);
        }
    }
}