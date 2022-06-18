using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float duration;
    public void Die()
    {
        StartCoroutine(LoadDieLevel());
    }

    private IEnumerator LoadDieLevel()
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene("Dead Scene");
    }
}
