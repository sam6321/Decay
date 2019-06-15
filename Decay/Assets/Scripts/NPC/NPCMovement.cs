using UnityEngine;
using UnityEngine.Events;

public class NPCMovement : MonoBehaviour
{
    [System.Serializable]
    public class OnArriveEvent : UnityEvent<Vector2, NPCMovement> { }

    [SerializeField]
    private float forwardMovementForce = 100f;

    [SerializeField]
    private float horizontalForwardMovementForce = 50f;

    [SerializeField]
    private float rotationTorqueMultiplier = 50.0f;

    [SerializeField]
    private float distanceSlowdownThreshold = 5.0f; // When within 5 units of target, begin slowing down

    [SerializeField]
    private float targetArrivedDistance = 0.25f; // When less than this distance from the target, consider us "arrived"
    public float TargetArrivedDistance { get => targetArrivedDistance; set => targetArrivedDistance = value; }

    [SerializeField]
    private float maxAngleDifference = 10; // Need to be pointing within this many degrees of our target or we'll try and turn towards it;

    [SerializeField]
    private OnArriveEvent onArrive = new OnArriveEvent();
    public OnArriveEvent OnArrive => onArrive;

    private new Rigidbody2D rigidbody2D;

    private Vector2 movement = new Vector2();
    public Vector2? MovementTarget { get; set; }

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Update movement to move toward the MovementTarget
        if(MovementTarget.HasValue)
        {
            if(Vector2.Distance(transform.position, MovementTarget.Value) < targetArrivedDistance)
            {
                // Arrived at target
                onArrive.Invoke(MovementTarget.Value, this);
                MovementTarget = null;
            }
            else
            {
                // Moving to target
                Vector2 offset = MovementTarget.Value - (Vector2)transform.position;

                float distance = offset.magnitude;
                Vector2 direction = offset.normalized;

                // Angle to target
                float angle = Vector2.SignedAngle(transform.up, direction);
                if(Mathf.Abs(angle) > maxAngleDifference)
                {
                    // Apply horizontal movement to rotate in direction of target
                    movement.x = -Mathf.Sign(angle);
                }
                else
                {
                    movement.x = 0;
                }

                // Move forwards towards the target, slowing down as we get closer
                movement.y = Mathf.Lerp(0.0f, 1.0f, Mathf.InverseLerp(0.0f, distanceSlowdownThreshold, distance));
            }
        }
        else
        {
            movement.Set(0, 0);
        }

        // Debug move to cursor
        if(Input.GetMouseButtonDown(0))
        {
            MovementTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void FixedUpdate()
    {

        if (movement.y != 0)
        {
            float force = movement.y < 0 ? movement.y * 0.2f : movement.y;
            rigidbody2D.AddForce(transform.up * forwardMovementForce * force * Time.fixedDeltaTime);
        }
        else if (movement.x != 0)
        {
            rigidbody2D.AddForce(transform.up * horizontalForwardMovementForce * Mathf.Abs(movement.x) * Time.fixedDeltaTime);
        }

        if (movement.x != 0)
        {
            rigidbody2D.AddTorque(-movement.x * rotationTorqueMultiplier * Time.fixedDeltaTime);
        }
    }
}
