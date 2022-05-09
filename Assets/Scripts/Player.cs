using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
            if (Game.IsPaused)
                FindObjectOfType<Pause>().Resume();
            else
                FindObjectOfType<Pause>().Stop();

        GetComponent<Rigidbody2D>().simulated = !Game.IsPaused;
        if (Game.IsPaused)
            return;
        var right = Input.GetAxis("Horizontal") * Time.deltaTime * 10 * transform.right;
        transform.position += right;
        
        
    }
}