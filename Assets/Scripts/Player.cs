using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Vector2 Position => transform.position;
    private float FeetRadius => 0.5f;
    public Vector2 FeetHigh => transform.GetChild(0).position + Vector3.up * FeetRadius;
    public Vector2 FeetLow => transform.GetChild(0).position - Vector3.up * FeetRadius;
    public Vector2 Feet => transform.GetChild(0).position;
    public Vector2 Head => transform.GetChild(1).position;
    private float JumpVelocity = 0;
    public Game game;

    void Update()
    {
        var right = Input.GetAxis("Horizontal") * 10 * Time.deltaTime;
        
        var isGrounded = game.IsPlayerGrounded(this);
        
        if (!isGrounded)
        {
            transform.position += Vector3.right * right;
            JumpVelocity -= 2*Time.deltaTime;
        }
        else
        {
            JumpVelocity = 0;
            var dy = game.CurrentPlatform.Evaluate(FeetLow + Vector2.right * right)
                     - game.CurrentPlatform.Evaluate(FeetLow);
            transform.position += new Vector3(right, dy);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                transform.position += (Vector3) (FeetLow - new Vector2(FeetLow.x, game.CurrentPlatform.Evaluate(FeetLow)));
                JumpVelocity = 3;
            }
        }
        transform.position += Vector3.up * JumpVelocity * Time.deltaTime;
        
        
    }
}