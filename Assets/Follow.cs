using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform Target;
    void Update()
    {
        var delta = Target.position - transform.position;
        var dx = delta.x;
        transform.position += Vector3.right * dx * Time.deltaTime;
        //transform.position = new Vector3(Target.position.x, Target.position.y, -10);
    }
}
