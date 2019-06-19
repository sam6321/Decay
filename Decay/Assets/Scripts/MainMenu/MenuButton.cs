using UnityEngine;

public class MenuButton : MonoBehaviour
{
    [SerializeField]
    Sprite offSprite;

    [SerializeField]
    Sprite onSprite;

    [SerializeField]
    AudioClip mouseOverAudio;

    private SpriteRenderer spriteRenderer;
    protected AudioSource audioSource;

    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void OnMouseOver()
    {
        if(spriteRenderer.sprite != onSprite)
        {
            audioSource.PlayOneShot(mouseOverAudio);
            spriteRenderer.sprite = onSprite;
        }
    }

    void OnMouseExit()
    {
        if(spriteRenderer.sprite != offSprite)
        {
            spriteRenderer.sprite = offSprite;
        }
    }
}
