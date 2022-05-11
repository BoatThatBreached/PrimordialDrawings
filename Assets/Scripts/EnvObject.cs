using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnvObject : MonoBehaviour
{
    [SerializeField] public EnvType type;
    public SelfDraw draw;
    public Material wood;
    public Material blood;
    public GameObject colliderPref;
    private List<LineRenderer> Renderers;
    private List<Vector3> Centers;
    public CircleCollider2D circleCollider;
    private Camera cam;
    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        Renderers = new List<LineRenderer>();
        Centers = new List<Vector3>();
        Physics2D.IgnoreCollision(circleCollider, FindObjectOfType<PlayerMovement>().playerCollider);
        switch (type)
        {
            case EnvType.Sprout:
                StartCoroutine(DrawSprout());
                break;
            case EnvType.Saddle:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private IEnumerator DrawSprout()
    {
        var dx = 0.05f;
        var dt = 0.04f;
        var pos = transform.position;
        draw.CreateBrush();
        for (var x = 0.5f; x > -0.7f; x -= dx)
        {
            draw.AddPoint(new Vector2(x+pos.x, -3*Mathf.Pow(x+0.3f, 4)+1+pos.y));
            yield return new WaitForSeconds(dt);
        }
        AddRenderer(wood, false);
        
        draw.CreateBrush();
        for (var x = 0.46f; x < 1.3f; x += dx)
        {
            draw.AddPoint(new Vector2(x+pos.x, -3*Mathf.Pow(x-1.038f, 2)+1+pos.y));
            yield return new WaitForSeconds(dt);
        }
        AddRenderer(wood, false);
        draw.CreateBrush();
        for (var x = 0.5f; x > -0.2f; x -= dx)
        {
            draw.AddPoint(new Vector2(x+pos.x, -5*Mathf.Pow(x+0.3f, 4)+1.8f+pos.y));
            yield return new WaitForSeconds(dt);
        }
        AddRenderer(wood, false);
        circleCollider.radius = 1.04f;
        circleCollider.offset = new Vector2(0.25f, 0.77f);
    }

    private void AddRenderer(Material material, bool addCollider)
    {
        var path = Enumerable
            .Range(0, draw.currentLineRenderer.positionCount)
            .Select(i => draw.currentLineRenderer.GetPosition(i))
            .ToList();
        draw.currentLineRenderer.material = material;
        Renderers.Add(draw.currentLineRenderer);
        Centers.Add(path.Aggregate((p1, p2)=>p1+p2)/path.Count());
        draw.currentLineRenderer.transform.SetParent(transform, true);
        if (!addCollider)
            return;
        for (var i = 0; i < path.Count() - 1; i++)
        {
            var center = (path[i] + path[i + 1]) / 2;
            var delta = path[i + 1] - path[i];
            var len = delta.magnitude;
            var terr = Instantiate(colliderPref, draw.currentLineRenderer.transform);
            terr.transform.position = center;
            terr.transform.localScale = new Vector3(len, 0.2f, 1);
            terr.transform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(delta.y, delta.x)/Mathf.PI*180f);
        }
    }

    private void FixedUpdate()
    {
        if (Game.IsPaused)
            return;

        for (var i = 0; i < Renderers.Count; i++)
        {
            var c = Centers[i];
            var t = Time.time;
            var a = 10f;
            var b = 10f;
            for (var j = 0; j < Renderers[i].positionCount; j++)
            {
                var delta = Renderers[i].GetPosition(j) - c;
                delta *= 1 + (Game.Leaf(Mathf.Cos(t+j/a)) - Game.Leaf(Mathf.Cos(t - Time.fixedDeltaTime+j/a)))/b;
                Renderers[i].SetPosition(j, c+delta);
            }
        }
    }

    private void Update()
    {
        if (Game.IsPaused)
            return;
        if(Input.GetMouseButtonDown(1)&&circleCollider.OverlapPoint(cam.ScreenToWorldPoint(Input.mousePosition)))
        {
            print("Oof!");
            if (PlayerInfo.Studied.Contains(type))
            {
                Use();
            }
            else
            {
                PlayerInfo.Studied.Add(type);
                PlayerInfo.ChosenType = type;
                Game.RefreshUI();
            }
        }
    }

    private void Use()
    {
        switch (type)
        {
            case EnvType.Sprout:
                StartCoroutine(EnlargeSprout());
                break;
            default:
                break;
        }
    }

    private IEnumerator EnlargeSprout()
    {
        var dy = 0.05f;
        var dt = 0.05f;
        var pos = transform.position;
        //draw.CreateBrush();
        for (var y = 10f; y < 20f; y += dy)
        {
            if(y%1f<0.01f)
                draw.CreateBrush();
            draw.AddPoint(new Vector2(20*Mathf.Sin(20*y)/y+pos.x+0.3f, y+pos.y-10));
            yield return new WaitForSeconds(dt);
            if(y%1f>0.94f)
                AddRenderer(wood, true);
        }
        //AddRenderer(wood, true);
        
    }
}