using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject level;
    public void Escape()
    {
        Game.IsPaused = false;
        SceneManager.LoadScene("MenuScene");
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
        
    }

    public void EnterSettings() => SceneManager.LoadScene("SettingsScene");

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
