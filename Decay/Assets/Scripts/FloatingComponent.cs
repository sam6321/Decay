using UnityEngine;
using UnityEngine.Events;
using cakeslice;

[RequireComponent(typeof(Outline), typeof(SpriteRenderer), typeof(Collider2D))]
public class FloatingComponent : MonoBehaviour
{
    [System.Serializable]
    public class OnComponentPickupEvent : UnityEvent<FloatingComponent> { };

    [SerializeField]
    private ShipComponent component;
    public ShipComponent ShipComponent => component;

    [SerializeField]
    private float pickupDistance = 50.0f;

    [SerializeField]
    private OnComponentPickupEvent onComponentPickup = new OnComponentPickupEvent();
    public OnComponentPickupEvent OnComponentPickup => onComponentPickup;

    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Outline outline;
    private Collider2D collider2d;
    private bool mouseOver = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        collider2d = GetComponent<Collider2D>();
        outline = GetComponent<Outline>();
        outline.eraseRenderer = true;
    }

    private void OnMouseOver()
    {
        if(outline.eraseRenderer && Vector2.Distance(collider2d.ClosestPoint(player.transform.position), player.transform.position) < pickupDistance)
        {
            outline.eraseRenderer = false;
            mouseOver = true;
        }
    }

    private void OnMouseDown()
    {
        // TODO: Add to boat somehow
        if(mouseOver)
        {
            onComponentPickup.Invoke(this);
            Destroy(gameObject);
        }
    }

    private void OnMouseExit()
    {
        outline.eraseRenderer = true;
        mouseOver = false;
    }
}
