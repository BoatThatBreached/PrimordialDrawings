using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Exit() => Application.Quit();
    public void Play() => SceneManager.LoadScene("Levels");
    //public void Play() => SceneManager.LoadScene("Test Level");
    public void EnterSettings() => SceneManager.LoadScene("SettingsScene");

    // public void Start()
    // {
    //     File.Delete(PlayerInfo.Path);
    //     PlayerInfo.Save();
    // }
}
