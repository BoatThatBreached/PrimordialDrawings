
using System.IO;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public void Exit() => Application.Quit();

    public void NewGame()
    {
        var dir = Directory.GetFiles(PlayerInfo.Path);
        foreach(var f in dir)
            File.Delete(f);
        Play();
    }

    public void Play() => FindObjectOfType<LevelLoader>().SelectScene("Levels");
    public void EnterSettings() => FindObjectOfType<LevelLoader>().SelectScene("SettingsScene");
}
