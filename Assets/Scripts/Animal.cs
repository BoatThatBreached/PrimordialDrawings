using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace DefaultNamespace
{
    public class Animal : MonoBehaviour
    {
        public string type;
        public bool saddled;
        public GameObject leftWall;
        public GameObject rightWall;
        public SavedEntry body;

        private float dx;

        private void Start()
        {
            body.Animate(PlayerInfo.Pass);
            body.Click = () => Saddle();
            dx = 0.1f;
        }

        private void Saddle()
        {
            if (PlayerInfo.CurrentType == "saddle" && type == "buffalo")
            {
                saddled = true;
                var obj = Instantiate(Resources.Load<GameObject>
                    ("Prefabs/your_saddle)).GetComponent<SavedEntry>();"), gameObject.transform, true);
                obj.transform.position = transform.position;
                obj.GetComponent<SavedEntry>().Animate(PlayerInfo.Pass);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name == "Player")
            {
                if (type == "buffalo" && saddled)
                    return;
                other.gameObject.GetComponent<PlayerMovement>().Die();
            }
            else if (other.gameObject == rightWall)
            {
                if (type == "tiger")
                    dx = -dx;
                else if (type == "buffalo")
                    transform.position = leftWall.transform.position;
            }
            else if (other.gameObject == leftWall && type == "tiger")
                dx = -dx;
            else if (other.gameObject.name == "spear" && type == "tiger")
                Destroy(gameObject);
        }

        private void Update()
        {
            transform.position += new Vector3(dx, 0, 0);
        }
    }
}