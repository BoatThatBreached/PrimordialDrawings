using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Exit() => Application.Quit();
    public void Play() => SceneManager.LoadScene("GameScene");
    public void EnterSettings() => SceneManager.LoadScene("SettingsScene");
}
