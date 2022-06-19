using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace Game_Objects
{
    public class Water : MonoBehaviour
    {
        public Transform waterPointsContainer;
        public GameObject wavePref;
        private List<SavedEntry> _waves;
        private List<float> _lengths;
        public float amplitude;
        public float speed;
        public Transform boat;
        public bool strongStream;
        private List<Vector3> _points;
        private int _waveIndex;
        public bool withPlayer;
        public float streamSpeed;

        public void Start()
        {
            _waves = new List<SavedEntry>();
            _lengths = new List<float>();
            _points = new List<Vector3>();
            _waveIndex = 0;
            foreach (Transform child in waterPointsContainer)
            {
                Destroy(child.GetComponent<SpriteRenderer>());
                _points.Add(child.transform.position);
            }

            for (var i = 0; i < _points.Count - 1; i++)
            {
                var wave = Instantiate(wavePref, transform);
                var delta = _points[i + 1] - _points[i];
                wave.transform.position = (_points[i] + _points[i + 1]) / 2;
                wave.transform.localEulerAngles = new Vector3(0, 0, -Mathf.Atan2(delta.y, delta.x) / Mathf.PI * 180);
                _waves.Add(wave.GetComponent<SavedEntry>());
                _lengths.Add(delta.magnitude);
            }
        }

        public void Update()
        {
            for (var i = 0; i < _waves.Count; i++)
                _waves[i].Float(_lengths[i], speed, amplitude);
            if (boat == null)
                return;

            boat.position = boat.position.WithX(Mathf.Clamp(
                boat.position.x,
                _points[0].x,
                _points[_points.Count - 1].x));
            if (strongStream)
            {
                try
                {
                    var delta = _points[_waveIndex + 1] - _points[_waveIndex];
                    boat.position += delta * Time.deltaTime * streamSpeed;
                    if (boat.position.x > _points[_waveIndex].x)
                        _waveIndex++;
                }
                catch
                {
                    _waveIndex = 0;
                    if(withPlayer)
                    {
                        withPlayer = false;
                        var player = FindObjectOfType<Player>();
                        player.transform.SetParent(transform.parent);
                        player.onBoat = false;
                        player.myRigidbody.simulated = true;
                    }
                    Destroy(boat.gameObject);
                    boat = null;
                }
            }
            else
                boat.position += Mathf.Sin(Time.time) / 60 * Vector3.up;
        }
    }
}