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
    public string fireIndex;

    private void Start()
    {
        fireBase.Animate(PlayerInfo.Pass);
    }

    private void Update()
    {
        var dt = Time.deltaTime;
        if (burning)
        {
            fireBase.EndAshen(dt, startHeight, offset, speed);
            fireBase.Burn();
            
        }
        else
        {
            fireBase.Ashen(emberCount, dt, startHeight, offset, speed);
            if (Vector2.Distance(transform.position, player.transform.position) > 5f)
                return;
            
            // var maxLevel = Math.Max(int.Parse(PlayerInfo.ReadString("maxLvl")), PlayerInfo.CurrentLevel+1);
            // PlayerInfo.WriteString("maxLvl", maxLevel.ToString());
            // SceneManager.LoadScene("Levels");
            burning = true;
        }
    }
}