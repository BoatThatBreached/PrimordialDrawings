using System;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

namespace Drawing
{
    public class RitualOriginal : MonoBehaviour
    {
        //public Camera cam;
        //public string title;
        private void Awake()
        {
            var title = PlayerInfo.WantedToLearn;
            print(title);
            var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/{title}")).GetComponent<SavedEntry>();
            obj.transform.position = new Vector3(0, 0, 0);
            obj.Animate();

            GameObject.Find("Draw").GetComponent<RitualDraw>().original = obj.GetComponent<SavedEntry>();
            GameObject.Find("Draw").GetComponent<RitualDraw>().title = title;
        }
    }
}