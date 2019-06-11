using UnityEngine;
using Common;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{

    [SerializeField]
    private float maxRotationDiffMagnitude = 20.0f;

    [SerializeField]
    private float movementForce = 100.0f;

    [SerializeField]
    private float rotationTorqueMultiplier = 50.0f;

    private Rigidbody2D rigidbody2D;

    private Vector2 movement;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        Vector2 direction = movement.normalized;
        if (direction.magnitude > 0)
        {
            float angle = Vector2.SignedAngle(Vector2.up, direction);
            float diff = Mathf.DeltaAngle(transform.eulerAngles.z, angle);
            diff = MathExtensions.ClampMagnitude(diff, 0.0f, maxRotationDiffMagnitude);

            rigidbody2D.AddForce(transform.up * movementForce * Time.fixedDeltaTime);
            rigidbody2D.AddTorque(diff * rotationTorqueMultiplier * Time.fixedDeltaTime);
        }
    }
}
