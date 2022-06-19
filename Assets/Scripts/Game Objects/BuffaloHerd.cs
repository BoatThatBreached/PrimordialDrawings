using System.Collections.Generic;
using UnityEngine;

public class BuffaloHerd : MonoBehaviour
{
    public Transform leftEdge;
    public Transform rightEdge;
    private List<SavedEntry> _oxen;
    public SavedEntry oxPref;

    public Player player;

    private void Start()
    {
        _oxen = new List<SavedEntry>();
        for (var i = 0; i < 8; i++)
            _oxen.Add(Instantiate(oxPref, transform).GetComponent<SavedEntry>());

        for (var i = 0; i < _oxen.Count; i++)
        {
            var ox = _oxen[i];
            ox.transform.position = 1f / _oxen.Count * (leftEdge.position * i + rightEdge.position * (_oxen.Count - i));
            //_speeds[i] = Random.Range(0f, Mathf.PI);
            ox.Animate(PlayerInfo.Pass);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (OverlapPlayer())
        {
            if (player.saddled)
            {
                player.held.localPosition = new Vector3(0.5f, 0, 0);
                var minDist = 100000f;
                var ridOx = _oxen[0];
                foreach (var ox in _oxen)
                {
                    var dist = Vector3.Distance(ox.transform.position, player.transform.position);
                    if (!(dist < minDist)) 
                        continue;
                    ridOx = ox;
                    minDist = dist;
                }

                player.transform.SetParent(ridOx.transform);
                player.myRigidbody.simulated = false;
            }

            else
                player.Die();
        }


        var ind = 0f;
        foreach (var ox in _oxen)
        {
            ind += 0.4f;
            ox.transform.position += Time.deltaTime *
                                     (16 * Waves(Time.time) * Vector3.right +
                                      7 * Mathf.Sin(9 * Time.time + ind) * Vector3.up);
            if (!(ox.transform.position.x > rightEdge.position.x)) 
                continue;
            if (ox.transform.childCount > 0)
            {
                player.myRigidbody.simulated = true;
                player.transform.SetParent(transform.parent);
            }
            ox.transform.position = leftEdge.position;
        }
    }

    private static float Waves(float x) => 3.5f + Mathf.Sin(4 * x) * (-1 + Mathf.Sin(4 * x));

    private bool OverlapPlayer()
    {
        var pos = player.transform.position;
        return pos.x > leftEdge.position.x && pos.x < rightEdge.position.x &&
               Mathf.Abs(transform.position.y - pos.y) < 3;
    }
}