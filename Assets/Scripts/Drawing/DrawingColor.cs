using System;
using UnityEngine;

namespace Drawing
{
    public class DrawingColor : MonoBehaviour
    {
        public string material;
        private SpriteRenderer cursorSprite;
        public bool chosen;
        private DrawingColor other;

        private void Start()
        {
            cursorSprite = GameObject.Find("Cursor").GetComponent<SpriteRenderer>();
        }

        private void OnMouseDown()
        {
            cursorSprite.color = material switch
            {
                "blood" => new Color(0.4078431f, 0, 0),
                "wood" => new Color(1, 0.5714479f, 0),
                "earth" => Color.black,
                _ => new Color(1, 1, 1)
            };
            chosen = true;
            other.chosen = false;
        }

        private void Update()
        {
            if (!chosen)
                return;
            var scale = PlayerInfo.Paints[material];
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}