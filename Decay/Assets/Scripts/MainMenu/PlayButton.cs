using UnityEngine;

public class PlayButton : MenuButton
{
    [SerializeField]
    AudioClip mouseDownAudio;

    [SerializeField]
    private GameObject setShipCount;

    void OnMouseDown()
    {
        audioSource.PlayOneShot(mouseDownAudio);
        setShipCount.SetActive(true);
    }
}
