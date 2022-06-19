using System.IO;
using Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Levels : MonoBehaviour
{
    public Transform grid;

    public void Start()
    {
        var maxLevel = File.Exists($"{PlayerInfo.Path}/maxLvl.prim")
            ? int.Parse(PlayerInfo.ReadString("maxLvl"))
            : 4;
        PlayerInfo.WriteString("maxLvl", maxLevel.ToString());
        foreach (Transform child in grid)
        {
            if (int.Parse(child.name) <= maxLevel)
                continue;
            child.GetComponent<Image>().color = Color.red;
            child.GetComponent<Button>().enabled = false;
        }
    }

    public void SelectLevel(int number)
    {
        print(number);
        PlayerInfo.CurrentLevel = number;
        PlayerInfo.InitLevelInfo(number);
        SceneManager.LoadScene($"Level {number}");
    }
}
