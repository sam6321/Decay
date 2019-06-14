using UnityEngine;
using cakeslice;

public class FloatingComponent : MonoBehaviour
{
    [SerializeField]
    private float pickupDistance = 50.0f;

    [SerializeField]
    private float timeScale = 1.0f;

    public FloatingComponentSpawner.SpawnTag SpawnTag { get; set; }

    private GameObject player;
    private Outline outline;
    private Collider2D collider2d;
    private PositionFollow positionFollow;
    private ShipComponent shipComponent;

    private bool mouseOver = false;
    private float offset;
    private Vector2 basePosition;

    private SpriteMask mask;
    private const float maskDeltaMagnitude = 0.0001f;
    private const float maskMax = 0.8f;
    private const float maskMin = 0.6f;
    private float maskDelta = maskDeltaMagnitude;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        collider2d = GetComponent<Collider2D>();
        outline = GetComponent<Outline>();
        positionFollow = GetComponent<PositionFollow>();
        shipComponent = GetComponent<ShipComponent>();
        outline.eraseRenderer = true;

        offset = Random.Range(0, 100);
        basePosition = transform.position;
        mask = GetComponent<SpriteMask>();
        if(mask)
        {
            mask.alphaCutoff = UnityEngine.Random.Range(maskMin, maskMax);
        }
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

        if(mask)
        {
            mask.alphaCutoff += maskDelta;
            if(mask.alphaCutoff > maskMax)
            {
                maskDelta = -maskDeltaMagnitude;
            }
            else if(mask.alphaCutoff < maskMin)
            {
                maskDelta = maskDeltaMagnitude;
            }
        }
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
            TryAttachToShip(player.GetComponent<ShipStructure>());
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

    public bool TryAttachToShip(ShipStructure structure)
    {
        // Try attaching this to the ship. If this returns false, then the ship
        // rejected the attachment (possibly because it already had a component of that type, or it hit a max number of components)
        if (enabled && shipComponent.Attach(structure))
        {
            // Attach was successful, so remove this from the spawner and 
            // disable the floating behaviours

            // Release from the spawner, as this is now owned by the ship
            SpawnTag.spawner.Remove(this);

            // Disable the FloatingComponent, as this has been attached to the ship
            collider2d.enabled = false;
            // Set the layer to indicate we're now owned by the ship
            gameObject.layer = LayerMask.NameToLayer("ShipComponent");
            enabled = false;
            outline.eraseRenderer = true;
            mouseOver = false;
            mask.alphaCutoff = 0;
        }

        return false;
    }
}
