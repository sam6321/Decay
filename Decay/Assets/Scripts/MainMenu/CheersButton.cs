using UnityEngine;

public class CheersButton : MenuButton
{
    [SerializeField]
    private GameObject attributions;

    [SerializeField]
    AudioClip mouseDownAudio;

    void OnMouseUp()
    {
        audioSource.PlayOneShot(mouseDownAudio);
        attributions.SetActive(true);
    }
}
