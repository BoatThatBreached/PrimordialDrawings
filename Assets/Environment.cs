using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    void Start()
    {
        transform.GetChild(0).GetComponent<SavedEntry>().Animate(PlayerInfo.Pass);
    }
}
