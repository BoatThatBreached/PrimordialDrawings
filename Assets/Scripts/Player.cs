using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 Position => transform.position;
    public Vector2 Feet => transform.GetChild(0).position;
    public Vector2 Head => transform.GetChild(1).position;
    private float VelocityUp = 0;
    public Game game;
    void Update()
    {
        var right = Input.GetAxis("Horizontal") * 10 * Time.deltaTime;
        var isGrounded = game.IsPlayerGrounded(this);
        if (isGrounded)
            VelocityUp = 0;
        if (isGrounded&&Input.GetKeyDown(KeyCode.Space))
            VelocityUp += 20f;
        print(isGrounded);
        VelocityUp += game.IsPlayerGrounded(this) ? 0 : -Time.deltaTime*10;
        
        transform.position += new Vector3(right, VelocityUp*Time.deltaTime);
        
    }
}
