using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class Cursor : MonoBehaviour
    {
        private static bool exists;

        private void Start()
        {
            if (exists)
                Destroy(gameObject);
            else
                exists = true;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }
}