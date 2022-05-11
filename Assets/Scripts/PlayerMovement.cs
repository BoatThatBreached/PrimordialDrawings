using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CapsuleCollider2D playerCollider;
    public GameObject envObject;
    public Camera cam;
    
    private void FixedUpdate()
    {
        if (Game.IsPaused)
            return;
        var right = Input.GetAxis("Horizontal") * Time.fixedDeltaTime * 10 * transform.right;
        transform.position += right;
        if (Input.GetKeyDown(KeyCode.Space)
            && IsGrounded())
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 400));
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
        if(Input.GetMouseButtonDown(1)
            &&PlayerInfo.EnoughResources()
        )
        {
            var obj = Instantiate(envObject, cam.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity).GetComponent<EnvObject>();
            obj.type = PlayerInfo.ChosenType;
            PlayerInfo.Spend();
            Game.RefreshUI();
        }
    }

    private bool IsGrounded() => playerCollider.IsTouchingLayers(LayerMask.GetMask("Terrain"));
}