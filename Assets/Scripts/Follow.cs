using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    public Vector2 offset;
    void Update()
    {
        var delta = Target.position - transform.position;
        var dx = delta.x+offset.x;
        var dy = delta.y+offset.y;
        transform.position += Vector3.right * dx * Time.deltaTime + Vector3.up * dy * Time.deltaTime*3;
    }
}