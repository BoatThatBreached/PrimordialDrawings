using UnityEngine;
using UnityEngine.SceneManagement;

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
        PlayerInfo.LastPlayerPos = FindObjectOfType<Player>().transform.position;
        PlayerInfo.Save();
        SceneManager.LoadScene("DrawScene");
    }
}