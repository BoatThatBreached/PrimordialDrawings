using UnityEngine;

namespace DefaultNamespace
{
    public class Fire : MonoBehaviour
    {
        private bool used;

        private void OnCollisionEnter(Collision other)
        {
            if (used)
                return;
            if (other.gameObject.name == "Player")
            {
                PlayerInfo.Paints["earth"] = 1;
                PlayerInfo.Paints["blood"] = 1;
                PlayerInfo.Paints["wood"] = 1;
                GameObject.Find("Player").GetComponent<PlayerMovement>().lastFire = gameObject;
                used = true;
            }
        }
    }
}