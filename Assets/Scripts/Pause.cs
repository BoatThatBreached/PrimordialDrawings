using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject level;
    public void Escape()
    {
        Game.IsPaused = false;
        PlayerInfo.Save();
        FindObjectOfType<LevelLoader>().SelectScene("MenuScene");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        level.SetActive(true);
        Game.IsPaused = false;
    }
    public void Stop()
    {
        pauseMenu.SetActive(true);
        level.SetActive(false);
        Game.IsPaused = true;
    }

    public void Restart()
    {
        PlayerInfo.ClearCurrentLevel();
        PlayerInfo.InitLevelInfo(PlayerInfo.CurrentLevel);
        FindObjectOfType<LevelLoader>().SelectScene($"Level {PlayerInfo.CurrentLevel}");
    }

    public void EnterSettings() => FindObjectOfType<LevelLoader>().SelectScene($"SettingsScene");

    private void Update()
    {
        TryEscape();
    }
    private void TryEscape()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;
        if(Game.IsPaused)
            Resume();
        else
            Stop();
    }
}
