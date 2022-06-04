using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CapsuleCollider2D playerCollider;
    public Camera cam;
    //public GameObject colors;
    public List<SavedEntry> spawnedObjects;

    private void Start()
    {
    }

    public void Die()
    {
        //pizdec...
    }

    private void FixedUpdate()
    {
        if (Game.IsPaused)
            return;
        var right = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * 10 * transform.right;
        transform.position += right;
        //colors.transform.position = Camera.main.transform.position + new Vector3(-15, 8, 10);
    }

    private void SpawnObject(string type)
    {
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/your_{type}")).GetComponent<SavedEntry>();
        obj.transform.position = pos;
        obj.envType = type;

        obj.Click = PlayerInfo.Gradation.ContainsKey(type) ? () => SpawnObject(PlayerInfo.Gradation[type]) : _pass;
        spawnedObjects.Add(obj);
        obj.Animate();
    }

    private readonly Action _pass = () => { };

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            if (Game.IsPaused)
                FindObjectOfType<Pause>().Resume();
            else
                FindObjectOfType<Pause>().Stop();
        GetComponent<Rigidbody2D>().simulated = !Game.IsPaused;
        if (Game.IsPaused)
            return;
        if (Input.GetMouseButtonDown(1)&&PlayerInfo.CanSpawnCurrentType)
        {
            SpawnObject(PlayerInfo.CurrentType);
        }
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     PlayerInfo.Learned.Add("sprout");
        //     PlayerInfo.Learned.Add("boat");
        //     PlayerInfo.Learned.Add("buffalo leg");
        //     PlayerInfo.Learned.Add("buffalo");
        //     PlayerInfo.Learned.Add("tiger leg");
        //     PlayerInfo.Learned.Add("tiger");
        //     PlayerInfo.Learned.Add("spear");
        //     PlayerInfo.Learned.Add("tree");
        //     PlayerInfo.Learned.Add("creatureLeg");
        // }
        // if (Input.GetKeyDown(KeyCode.K))
        // {
        //     foreach(var o in spawnedObjects)
        //         StartCoroutine(o.Change());
        // }

        // var delayedActions = new List<Action>();
        // foreach (var o in spawnedObjects.Where(o => o.Clicked()))
        // {
        //     print(o.envType);
        //     if (PlayerInfo.Learned.Contains(o.envType))
        //         delayedActions.Add(o.Click);
        //     else
        //         SceneManager.LoadScene("DrawScene");
        // }

        // foreach (var act in delayedActions)
        //     act();
        

        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Finish")))
            SceneManager.LoadScene("MenuScene");
    }
}