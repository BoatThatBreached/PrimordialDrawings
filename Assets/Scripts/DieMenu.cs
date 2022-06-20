using UnityEngine;

public class DieMenu : MonoBehaviour
{
    public void Restart()
    {
        PlayerInfo.ClearCurrentLevel();
        PlayerInfo.InitLevelInfo(PlayerInfo.CurrentLevel);
        FindObjectOfType<LevelLoader>().SelectScene($"Level {PlayerInfo.CurrentLevel}");
    }
    public void ToMenu()
    {
        FindObjectOfType<LevelLoader>().SelectScene("MenuScene");
    }

    public void Respawn()
    {
        FindObjectOfType<LevelLoader>().SelectScene($"Level {PlayerInfo.CurrentLevel}");
    }
}
