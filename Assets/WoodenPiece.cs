using UnityEngine;

public class WoodenPiece : MonoBehaviour
{
    private SavedEntry _selected;

    public void Select(SavedEntry obj)
    {
        _selected = obj;
    }

    private void Update()
    {
        if (_selected == null)
            return;
        transform.localScale = _selected.Volume*Vector3.one;
        if (!(_selected.Volume < 1e-6)) 
            return;
        var group = transform.parent.GetComponent<CircleLayoutGroup>();
        group.Init(group.count-1);
        
    }
}
