using UnityEngine;

public class ExitButton : MenuButton
{
    [SerializeField]
    AudioClip mouseDownAudio;

    void OnMouseDown()
    {
        audioSource.PlayOneShot(mouseDownAudio);
        Application.Quit();
    }
}
