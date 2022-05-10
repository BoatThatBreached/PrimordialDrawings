using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CapsuleCollider2D playerCollider;

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
        
        var right = Input.GetAxis("Horizontal") * Time.deltaTime * 10 * transform.right;
        transform.position += right;
        if (Input.GetKeyDown(KeyCode.Space)
            && IsGrounded())
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 400));
    }

    private bool IsGrounded()
    {
        return playerCollider.IsTouchingLayers(LayerMask.GetMask("Terrain"));
    }
}