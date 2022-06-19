using System;
using UnityEngine;

namespace Game_Objects
{
    public class Fire : MonoBehaviour
    {
        public bool burning;
        public SavedEntry fireBase;
        public float speed;
        public float offset;
        public float startHeight;
        public int emberCount;
        public Player player;
        public int fireIndex;

        private void Start()
        {
            fireBase.Animate(PlayerInfo.Pass);
        }

        private void Update()
        {
            if (player == null)
                return;
            var dt = Time.deltaTime;
            if (burning)
            {
                fireBase.EndAshen(dt, startHeight, offset, speed);
                fireBase.Burn();
            }
            else
            {
                fireBase.Ashen(emberCount, dt, startHeight, offset, speed);
                if (Vector2.Distance(transform.position, player.transform.position) > 10f)
                    return;
                
                burning = true;
                PlayerInfo.FireIndex = Math.Max(fireIndex, PlayerInfo.FireIndex);
                PlayerInfo.Save();
            }
        }
    }
}