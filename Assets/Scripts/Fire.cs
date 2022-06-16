using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fire : MonoBehaviour
{
    public bool burning;
    public SavedEntry fireBase;
    public float speed;
    public float offset;
    public float startHeight;
    public int emberCount;
    public Player player;

    private void Start()
    {
        fireBase.Animate(PlayerInfo.Pass);
    }

    private void Update()
    {
        if (burning)
        {
            fireBase.Burn();
            if (Vector2.Distance(transform.position, player.transform.position) > 5f)
                return;
            
            var maxLevel = Math.Max(int.Parse(PlayerInfo.ReadString("maxLvl")), PlayerInfo.CurrentLevel+1);
            PlayerInfo.WriteString("maxLvl", maxLevel.ToString());
            SceneManager.LoadScene("Levels");
        }
        else
            fireBase.Ashen(emberCount, Time.deltaTime, startHeight, offset, speed);
    }
}