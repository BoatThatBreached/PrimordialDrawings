using UnityEngine;

namespace Game_Objects
{
    public class MovingPlatform : MonoBehaviour
    {
        public GameObject Pair;
        private Player _player;
        private Vector3 _offset;

        private void Start()
        {
            _player = FindObjectOfType<Player>();
            _offset = _player.GetComponent<CapsuleCollider2D>().offset;
        }

        private void Update()
        {
            var playerPos = _player.transform.position + _offset;
            if (Mathf.Abs(playerPos.x - transform.position.x) < 5.5f
                && Mathf.Abs(playerPos.y - transform.position.y) < 1f)
            {
                transform.position += Vector3.down;
                Pair.transform.position += Vector3.up;
            }
        }
    }
}