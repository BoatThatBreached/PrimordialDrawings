using System;
using UnityEngine;

public class Spikes : MonoBehaviour
{
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
            _player.Die();
    }
}