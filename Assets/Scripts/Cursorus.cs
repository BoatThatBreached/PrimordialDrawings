
using UnityEngine;

    public class Cursorus : MonoBehaviour
    {
        private static bool exists;

        private void Start()
        {
            if (exists)
                Destroy(gameObject);
            else
                exists = true;
            Cursor.visible = false;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(pos.x, pos.y, 0);
        }
    }
