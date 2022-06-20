using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float duration;

    public void SelectScene(string scene)
    {
        StartCoroutine(Select(scene));
    }
    private IEnumerator Select(string scene)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene(scene);
    }
}
