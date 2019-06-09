using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject attributions;

    public void OnStartClicked()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickAttributions()
    {
        attributions.SetActive(!attributions.activeSelf);
    }

    public void OnExitClicked()
    {
        Application.Quit();
    }
}
