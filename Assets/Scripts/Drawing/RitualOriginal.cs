using System;
using UnityEngine;

namespace Drawing
{
    public class RitualOriginal : MonoBehaviour
    {
        public Camera cam;
        public string title;
        private void Start()
        {
            var pos = cam.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0;
            var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/{title}")).GetComponent<SavedEntry>();
            obj.transform.position = new Vector3(0, 0, 0);
            obj.Animate();

            GameObject.Find("Draw").GetComponent<RitualDraw>().original = obj.GetComponent<SavedEntry>();

        }
    }
}