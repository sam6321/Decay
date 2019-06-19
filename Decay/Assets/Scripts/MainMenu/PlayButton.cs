using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MenuButton
{
    [SerializeField]
    AudioClip mouseDownAudio;

    void OnMouseDown()
    {
        audioSource.PlayOneShot(mouseDownAudio);
        // GameObject.Find("GameManager").GetComponent<FadeManager>().StartFade(() => SceneManager.LoadScene("MainScene"));
        SceneManager.LoadScene("MainScene");
    }
}
