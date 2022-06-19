using Extensions;
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
        //private float dx;
        private int _direction;
        public float speed = 6;
        private Player _player;
        private Vector3 _offset;
        public bool walking;

        private void Start()
        {
            walking = false;
            _player = FindObjectOfType<Player>();
            _offset = _player.GetComponent<CapsuleCollider2D>().offset;
            body.Animate(() => walking = true);
            //body.Click = () => Saddle();
            //dx = 0.1f;
            _direction = 1;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject == _player.gameObject)
            {
                body.Animate(PlayerInfo.Pass);
                body.Click = () => Saddle();
                _direction = 1;
                //dx = 0.1f;
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
            if (!walking)
                return;
            transform.position += Time.deltaTime * speed * _direction * Vector3.right;
            if (transform.position.x >= rightX)
            {
                if (type == "tiger")
                {
                    //dx = -dx;
                    _direction *= -1;
                    transform.localEulerAngles -= 180 * Vector3.up;
                }
                else if (type == "buffalo")
                    transform.position = transform.position.WithX(leftX);
            }
            else if (transform.position.x <= leftX && type == "tiger")
            {
                //dx = -dx;
                _direction *= -1;
                transform.localEulerAngles += 180 * Vector3.up;
            }

            var playerPos = _player.transform.position + _offset;
            if (Mathf.Abs(playerPos.x - transform.position.x) < 5.5f
                && Mathf.Abs(playerPos.y - transform.position.y) < 1f)
                _player.Die();
        }
    }
}