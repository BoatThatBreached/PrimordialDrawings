using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public CapsuleCollider2D playerCollider;
    public Camera cam;
    public GameObject colors;

    private void FixedUpdate()
    {
        if (Game.IsPaused)
            return;
        var right = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * 10 * transform.right;
        transform.position += right;
        colors.transform.position = Camera.main.transform.position + new Vector3(-15, 8, 10);
    }

    private void SpawnObject(string type)
    {
        var pos = cam.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/{type}")).GetComponent<SavedEntry>();
        obj.transform.position = pos;
        obj.envType = type;

        obj.Click = PlayerInfo.Gradation.ContainsKey(type) ? () => SpawnObject(PlayerInfo.Gradation[type]) : _pass;
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerInfo.Learned.Add("sprout");
            PlayerInfo.Learned.Add("boat");
            PlayerInfo.Learned.Add("buffalo leg");
            PlayerInfo.Learned.Add("buffalo");
            PlayerInfo.Learned.Add("tiger leg");
            PlayerInfo.Learned.Add("tiger");
            PlayerInfo.Learned.Add("spear");
            PlayerInfo.Learned.Add("tree");
        }

        if (playerCollider.IsTouchingLayers(LayerMask.GetMask("Finish")))
            SceneManager.LoadScene("MenuScene");
    }
}