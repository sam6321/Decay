using UnityEngine;
using Common;

public class FloatingComponent : MonoBehaviour
{
    [SerializeField]
    private ShipComponent component;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = RandomExtensions.RandomElement(component.waterSprites);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
