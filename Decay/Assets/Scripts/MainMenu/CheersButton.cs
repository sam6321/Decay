using UnityEngine;

public class CheersButton : MenuButton
{
    [SerializeField]
    private GameObject attributions;

    [SerializeField]
    AudioClip mouseDownAudio;

    void OnMouseDown()
    {
        audioSource.PlayOneShot(mouseDownAudio);
        attributions.SetActive(!attributions.activeSelf);
    }
}
