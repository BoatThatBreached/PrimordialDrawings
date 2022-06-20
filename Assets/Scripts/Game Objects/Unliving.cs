using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game_Objects
{
    public class Unliving : MonoBehaviour
    {
        public SavedEntry body;

        public void Update()
        {
            if (!body.Clicked())
                return;
            PlayerInfo.PrevScene = SceneManager.GetActiveScene().name;
            PlayerInfo.WantedToLearn = body.envType;
            PlayerInfo.WantedScale = body.transform.localScale;
            PlayerInfo.Save();
            FindObjectOfType<LevelLoader>().SelectScene("DrawScene");
        }
    }
}