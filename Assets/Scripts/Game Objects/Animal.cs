using UnityEngine;

namespace Game_Objects
{
    public class Animal : MonoBehaviour
    {
        public string type;
        public bool saddled;
        public float leftX;
        public float rightX;
        public SavedEntry body;
        private float dx;
        private Player _player;
        private Vector3 _offset;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
            _offset = _player.GetComponent<CapsuleCollider2D>().offset;
            body.Animate(PlayerInfo.Pass);
            body.Click = () => Saddle();
            dx = 0.1f;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.name == "player 18-")
            {
                body.Animate(PlayerInfo.Pass);
                body.Click = () => Saddle();
                dx = 0.1f;
            }
            else if (other.gameObject.name == "spear" && type == "tiger")
                Destroy(gameObject);
        }

        private void Saddle()
        {
            // if (PlayerInfo.CurrentType == "saddle" && type == "buffalo")
            // {
            //     saddled = true;
            //     var obj = Instantiate(Resources.Load<GameObject>
            //         ("Prefabs/your_saddle)).GetComponent<SavedEntry>();"), gameObject.transform, true);
            //     obj.transform.position = transform.position;
            //     obj.GetComponent<SavedEntry>().Animate(PlayerInfo.Pass);
            // }
        }

        private void Update()
        {
            transform.position += new Vector3(dx, 0, 0);
            if (transform.position.x >= rightX)
            {
                if (type == "tiger")
                {
                    dx = -dx;
                    transform.localEulerAngles -= 180 * Vector3.up;
                }
                else if (type == "buffalo")
                    transform.position = new Vector3(leftX, transform.position.x, transform.position.z);
            }
            else if (transform.position.x <= leftX && type == "tiger")
            {
                dx = -dx;
                transform.localEulerAngles += 180 * Vector3.up;
            }

            var playerPos = _player.transform.position + _offset;
            if (Mathf.Abs(playerPos.x - transform.position.x) < 5.5f
                && Mathf.Abs(playerPos.y - transform.position.y) < 1f)
                _player.Die();
        }
    }
}