using Extensions;
using UnityEngine;

public class CircleLayoutGroup : MonoBehaviour
{
    public int radius;
    public int paddingAngle;
    public int spacing;
    public int sectorAngle;
    public int Count;
    public GameObject childPref;

    //private List<Transform> _group;

    public void Init(int childCount)
    {
        Count = childCount;
        Game.Clear(transform);
        var deltaAngle = childCount == 1 ? 0 : (float) sectorAngle / (childCount - 1);
        for (var i = 0; i < childCount; i++)
        {
            var child = Instantiate(childPref, transform).transform;
            child.localPosition = CirclePos(90 - paddingAngle - deltaAngle * i - spacing);
        }
    }

    private Vector3 CirclePos(float angle) => radius * new Vector3(Mathf.Cos(angle.Rad()), Mathf.Sin(angle.Rad()), 0);
}