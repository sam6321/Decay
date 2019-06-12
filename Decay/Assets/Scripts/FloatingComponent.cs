using UnityEngine;
using cakeslice;

public class FloatingComponent : MonoBehaviour
{
    [SerializeField]
    private ShipComponent component;
    public ShipComponent ShipComponent => component;

    [SerializeField]
    private float pickupDistance = 50.0f;

    [SerializeField]
    private float timeScale = 1.0f;

    public FloatingComponentSpawner Spawner { get; set; }

    private GameObject player;
    private SpriteRenderer spriteRenderer;
    private Outline outline;
    private Collider2D collider2d;
    private PositionFollow positionFollow;
    private bool mouseOver = false;

    private float offset;
    private Vector2 basePosition;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        collider2d = GetComponent<Collider2D>();
        outline = GetComponent<Outline>();
        positionFollow = GetComponent<PositionFollow>();
        outline.eraseRenderer = true;

        offset = Random.Range(0, 100);
        basePosition = transform.position;
    }

    private void Update()
    {
        float t = (Time.time + offset) * timeScale;
        Vector2 vectorOffset = new Vector2(
            Mathf.Sin(t) + Mathf.Cos(t / 2),
            Mathf.Cos(t * 2) - Mathf.Sin(t)
        );

        transform.eulerAngles = new Vector3(0, 0, Mathf.Rad2Deg * (Mathf.Sin(t / 2) * Mathf.Cos(t * 2)));
        transform.position = basePosition + vectorOffset;
    }

    private void OnMouseOver()
    {
        if(enabled && outline.eraseRenderer && Vector2.Distance(collider2d.ClosestPoint(player.transform.position), player.transform.position) < pickupDistance)
        {
            outline.eraseRenderer = false;
            mouseOver = true;
        }
    }

    private void OnMouseDown()
    {
        if(mouseOver && enabled)
        {
            // Release from the spawner, as this is now owned by the player
            Spawner.Remove(this);

            // Disable the FloatingComponent and send this component over to the player
            enabled = false;
            outline.eraseRenderer = true;
            mouseOver = false;

            // TODO: Send it properly, rather than doing this for fun
            player.GetComponent<ChainFollowParent>().Add(positionFollow);
            positionFollow.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        if (enabled)
        {
            outline.eraseRenderer = true;
            mouseOver = false;
        }
    }
}
