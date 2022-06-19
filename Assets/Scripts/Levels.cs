using System.IO;
using Extensions;
using TMPro;
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
            : 0;
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
        PlayerInfo.CurrentLevel = number;
        PlayerInfo.LastPlayerPos = new Vector3();
        var lastPos = File.Exists($"{PlayerInfo.Path}/lastPos_{PlayerInfo.CurrentLevel}.prim")
            ? PlayerInfo.ReadString($"lastPos_{PlayerInfo.CurrentLevel}").ToVector()
            : new Vector3();
        PlayerInfo.WriteString($"lastPos_{PlayerInfo.CurrentLevel}", lastPos.ToVectString());
        PlayerInfo.InitLevelInfo();
        SceneManager.LoadScene($"Level {number.ToString()}");
    }
}
