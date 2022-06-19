using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    //public CapsuleCollider2D playerCollider;
    public Rigidbody2D myRigidbody;
    public Camera cam;

    public GameObject lastFire;
    //public GameObject colors;
    private bool _armed;
    private Transform _spear;
    public List<SavedEntry> spawnedObjects;

    private void Start()
    {
    }

    public void Die()
    {
        Debug.Log("dead");
        transform.position = lastFire.transform.position;
    }

    private void FixedUpdate()
    {
        
        if (Game.IsPaused)
            return;
        //transform.position+=new Vector3(Input.GetAxis("Horizontal") * Time.fixedDeltaTime * 400, 0);
        myRigidbody.MovePosition(myRigidbody.position+new Vector2(Input.GetAxis("Horizontal") * Time.fixedDeltaTime * 10, 0));

        //var right = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * 10 * transform.right;
        //transform.position += right;
        //GetComponent<Rigidbody2D>().AddForce(new Vector2(Input.GetAxis("Horizontal") * Time.fixedDeltaTime * 500, 0));
    }

    private void SpawnObject(string type)
    {
        if (type == "saddle")
            return;
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);//-cam.transform.position;
        pos.z = -15;
        var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/your_{type}")).GetComponent<SavedEntry>();
        obj.transform.position = pos;
        obj.envType = type;
        if (type == "spear")
        {
            obj.transform.SetParent(transform);
            obj.Animate(() =>
            {
                _spear = obj.transform;
                _spear.localEulerAngles = new Vector3(0, 0, 45);
                _spear.transform.localPosition = new Vector3(4.7f, 0, 4.5f);
                _armed = true;
            });
        }
        else
        {
            // obj.Click = PlayerInfo.Gradation.ContainsKey(type)
            //     ? () => SpawnObject(PlayerInfo.Gradation[type])
            //     : PlayerInfo.Pass;
            // spawnedObjects.Add(obj);
            // obj.Animate(PlayerInfo.Pass);
        }
    }


    private void Update()
    {
        if (transform.position.y < -30)
        {
            Die();
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
            if (Game.IsPaused)
                FindObjectOfType<Pause>().Resume();
            else
                FindObjectOfType<Pause>().Stop();
        GetComponent<Rigidbody2D>().simulated = !Game.IsPaused;
        if (Game.IsPaused)
            return;
        //if (Input.GetMouseButtonDown(1) && PlayerInfo.CanSpawnCurrentType)
        {
            //SpawnObject(PlayerInfo.CurrentType);
        }

        if (_armed)
        {
            //spear.localEulerAngles = Mathf.Atan2(Input)Vector3.forward);
            var mpos = cam.ScreenToWorldPoint(Input.mousePosition) - cam.transform.position;
            var direction = Mathf.Atan2(mpos.y, mpos.x);
            _spear.localEulerAngles = direction / Mathf.PI * 180 * Vector3.forward;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _armed = false;
                _spear.SetParent(transform.parent);
                _spear.GetComponent<SavedEntry>().Move(direction);
            }
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
        


        // if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Finish")))
        //     SceneManager.LoadScene("MenuScene");
    }
}