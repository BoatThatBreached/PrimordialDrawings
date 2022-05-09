using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    
    public void Escape()
    {
        Game.IsPaused = false;
        SceneManager.LoadScene("MenuScene");
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Game.IsPaused = false;
    }
    public void Stop()
    {
        pauseMenu.SetActive(true);
        Game.IsPaused = true;
    }
    public void EnterSettings() => SceneManager.LoadScene("SettingsScene");
}
