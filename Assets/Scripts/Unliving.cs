using UnityEngine;
using UnityEngine.SceneManagement;

public class Unliving:MonoBehaviour
{
    public SavedEntry body;

    public void Start()
    {
        body.Animate(PlayerInfo.Pass);
    }

    public void Update()
    {
        if (body.Clicked())
        {
            if (PlayerInfo.Learned.Contains(body.envType))
                body.Click();
            else
            {
                //Destroy(body.gameObject);
                PlayerInfo.WantedToLearn = body.envType;
                print(PlayerInfo.WantedToLearn);
                SceneManager.LoadScene("DrawScene");
            }
        }
    }
    
}