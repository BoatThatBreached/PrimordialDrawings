using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public float speed;
    public Vector2 offset;

    private void Update()
    {
        if (target == null)
            return;
        var delta = target.position - transform.position;
        var dx = delta.x+offset.x;
        var dy = delta.y+offset.y;
        transform.position += Time.deltaTime*speed*(dx * Vector3.right  + dy * Vector3.up);
    }
}