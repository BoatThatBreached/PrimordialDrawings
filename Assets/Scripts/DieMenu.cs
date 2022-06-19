using UnityEngine;
using UnityEngine.SceneManagement;

public class DieMenu : MonoBehaviour
{
    public void Restart()
    {
        PlayerInfo.ClearCurrentLevel();
        PlayerInfo.InitLevelInfo(PlayerInfo.CurrentLevel);
        SceneManager.LoadScene($"Level {PlayerInfo.CurrentLevel}");
    }
    public void ToMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void Respawn()
    {
        SceneManager.LoadScene($"Level {PlayerInfo.CurrentLevel}");
    }
}
