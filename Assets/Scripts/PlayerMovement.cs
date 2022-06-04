using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CapsuleCollider2D playerCollider;

    public Camera cam;

    //public GameObject colors;
    private bool _armed;
    private Transform spear;
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
    }

    private void SpawnObject(string type)
    {
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/your_{type}")).GetComponent<SavedEntry>();
        obj.transform.position = pos;
        if (type == "spear")
        {
            obj.transform.SetParent(transform);
            
        }

        obj.envType = type;

        obj.Click = PlayerInfo.Gradation.ContainsKey(type)
            ? () => SpawnObject(PlayerInfo.Gradation[type])
            : PlayerInfo.Pass;
        spawnedObjects.Add(obj);
        obj.Animate(() =>
        {
            spear = obj.transform;
            spear.localEulerAngles = new Vector3(0, 0, 45);
            spear.transform.localPosition = new Vector3(4.7f, 0, 4.5f);
            _armed = true;
        });
    }


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
        if (Input.GetMouseButtonDown(1) && PlayerInfo.CanSpawnCurrentType)
        {
            SpawnObject(PlayerInfo.CurrentType);
        }

        if (_armed)
        {
            //spear.localEulerAngles = Mathf.Atan2(Input)Vector3.forward);
            var mpos = cam.ScreenToWorldPoint(Input.mousePosition)-cam.transform.position;
            var direction = Mathf.Atan2(mpos.y, mpos.x);
            spear.localEulerAngles = direction / Mathf.PI * 180 * Vector3.forward;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _armed = false;
                spear.SetParent(transform.parent);
                spear.GetComponent<SavedEntry>().Move(direction);
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach(var o in spawnedObjects)
                StartCoroutine(o.Change());
        }

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