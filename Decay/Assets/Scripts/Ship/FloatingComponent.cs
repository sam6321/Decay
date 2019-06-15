using UnityEngine;
using cakeslice;

public class FloatingComponent : MonoBehaviour
{
    [SerializeField]
    private float pickupDistance = 50.0f;
    public float PickupDistance => pickupDistance;

    [SerializeField]
    private float timeScale = 1.0f;

    [SerializeField]
    private int npcPriority = 0; // NPCs prefer to go after higher priority items
    public int NPCPriority => npcPriority;

    public FloatingComponentSpawner.SpawnTag SpawnTag { get; set; }

    private ShipStructure player;
    private Outline outline;
    private Collider2D collider2d;
    private Rigidbody2D rigidbody2D;
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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<ShipStructure>();
        collider2d = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
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

        if(rigidbody2D != null)
        {
            if(rigidbody2D.angularVelocity > 10.0f)
            {
                rigidbody2D.AddTorque(-0.1f);
            }
            else if(rigidbody2D.angularVelocity < -10.0f)
            {
                rigidbody2D.AddTorque(0.1f);
            }
            else
            {
                float desiredRot = Mathf.Rad2Deg * (Mathf.Sin(t / 2) * Mathf.Cos(t * 2)) - transform.eulerAngles.z;
                while(desiredRot > 360.0f)
                {
                    desiredRot -= 360.0f;
                }
                while(desiredRot < 0.0f)
                {
                    desiredRot += 360.0f;
                }
                if(Mathf.Abs(desiredRot - 360.0f) < desiredRot)
                {
                    desiredRot -= 360.0f;
                }
                rigidbody2D.AddTorque(desiredRot > 0.0f ? 0.1f : -0.1f);
            }
            
            var currPos = new Vector2(transform.position.x, transform.position.y);
            var scale = new Vector2(0.01f, 0.01f);
            var velocity = (basePosition + vectorOffset - currPos).normalized * scale;

            if(rigidbody2D.velocity.x > 0.1f)
            {
                velocity.x = -0.01f;
            }
            else if(rigidbody2D.velocity.x < -0.1f)
            {
                velocity.x = 0.01f;
            }

            if(rigidbody2D.velocity.y > 10.0f)
            {
                velocity.y = -0.01f;
            }
            else if(rigidbody2D.velocity.y < -10.0f)
            {
                velocity.y = 0.01f;
            }

            rigidbody2D.AddForce(velocity);
        }

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
        if(enabled && outline.eraseRenderer && CanPickup(player))
        {
            outline.eraseRenderer = false;
            mouseOver = true;
        }
    }

    private void OnMouseDown()
    {
        if(mouseOver && enabled)
        {
            TryAttachToShip(player);
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

    public bool CanPickup(ShipStructure structure)
    {
        return DistanceTo(structure) < pickupDistance;
    }

    public float DistanceTo(ShipStructure structure)
    {
        Vector2 closestShipPoint = structure.ClosestPoint(transform.position);
        Vector2 closestComponentPoint = collider2d.ClosestPoint(closestShipPoint);
        Debug.DrawLine(closestShipPoint, closestComponentPoint, Color.red, 1);
        return Vector2.Distance(closestShipPoint, closestComponentPoint);
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
            // Set the layer to indicate we're now owned by the ship
            gameObject.layer = LayerMask.NameToLayer("ShipComponent");
            enabled = false;
            outline.eraseRenderer = true;
            mouseOver = false;
            mask.alphaCutoff = 0;

            return true;
        }

        return false;
    }
}
