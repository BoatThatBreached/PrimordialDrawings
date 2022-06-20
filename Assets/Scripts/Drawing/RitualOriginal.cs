using UnityEngine;

namespace Drawing
{
    public class RitualOriginal : MonoBehaviour
    {
        private void Awake()
        {
            var title = PlayerInfo.WantedToLearn;
            var obj = Instantiate(Resources.Load<GameObject>($"Prefabs/{title}")).GetComponent<SavedEntry>();
            obj.transform.position = new Vector3(0, 0, 6);
            obj.transform.localScale = PlayerInfo.WantedScale;
            obj.Animate(PlayerInfo.Pass);

            GameObject.Find("Draw").GetComponent<RitualDraw>().original = obj.GetComponent<SavedEntry>();
            GameObject.Find("Draw").GetComponent<RitualDraw>().title = title;
        }
    }
}