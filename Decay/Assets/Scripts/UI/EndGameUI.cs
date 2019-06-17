using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameUI : MonoBehaviour
{
    public void OnPlayAgain()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
