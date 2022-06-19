using Extensions;
using UnityEngine;

public class CircleLayoutGroup : MonoBehaviour
{
    public int radius;
    public int paddingAngle;
    public int spacing;
    public int sectorAngle;
    public int count;
    public GameObject childPref;

    //private List<Transform> _group;

    public void Init(int childCount)
    {
        count = childCount;
        Game.Clear(transform);
        var deltaAngle = childCount == 1 ? 0 : (float) sectorAngle / (childCount - 1);
        for (var i = 0; i < childCount; i++)
        {
            var child = Instantiate(childPref, transform).transform;
            child.localPosition = CirclePos(90 - paddingAngle - deltaAngle * i - spacing);
        }
    }

    public void Select(SavedEntry obj) => transform.GetChild(0).GetComponent<WoodenPiece>().Select(obj);

    private Vector3 CirclePos(float angle) => radius * new Vector3(Mathf.Cos(angle.Rad()), Mathf.Sin(angle.Rad()), 0);
}