using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float duration;
    private static readonly int Start = Animator.StringToHash("Start");

    public void Die()
    {
        StartCoroutine(LoadDieLevel());
    }

    private IEnumerator LoadDieLevel()
    {
        var scene = SceneManager.GetActiveScene().name;
        transition.SetTrigger(Start);
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene("Dead Scene");
        print("kek");
        yield return new WaitForSeconds(duration);
        print("lol");
        SceneManager.LoadScene(scene);
    }
}
