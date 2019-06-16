using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private float forwardMovementForce = 50f;

    private float horizontalForwardMovementForce = 50f;

    private float rotationTorqueMultiplier = 1f;

    private Rigidbody2D rigidbody2D;
    private ShipStructure structure;

    private Vector2 movement = new Vector2();

    private float bowForce = 1.5f;
    private float oarForce = 1.05f;
    private float sternRotationMultiplier = 1.5f;
    private float massMultiplier = 0.25f;
    private float angularDragMultiplier = 5f;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        structure = GetComponent<ShipStructure>();
    }

    void Update()
    {
        movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    
    void FixedUpdate()
    {
        bool hasStern = structure.Stern != null;
        bool hasBow = structure.Bow != null;
        int numOars = structure.Oars.Count;
        int leftOars = numOars / 2;
        int rightOars = numOars / 2;
        int width = structure.Width;
        int numPlanks = structure.Planks.Count;
        rigidbody2D.mass = numPlanks * massMultiplier;
        rigidbody2D.angularDrag = numPlanks * angularDragMultiplier;

        if(numOars % 2 == 1)
        {
            leftOars++;
        }

        float leftForce = movement.y * forwardMovementForce;
        float rightForce = movement.y * forwardMovementForce;

        leftForce *= hasBow ? bowForce : 1f;
        rightForce *= hasBow ? bowForce : 1f;

        leftForce *= (leftOars + 1f) * oarForce;
        rightForce *= (rightOars + 1f) * oarForce;

        rigidbody2D.AddForce(transform.up * (leftForce + rightForce) * Time.fixedDeltaTime);

        if(!hasStern)
        {
            rigidbody2D.AddTorque((rightForce - leftForce) * width / 2f * rotationTorqueMultiplier * Time.fixedDeltaTime);
        }

        leftForce *= movement.x;
        rightForce *= -movement.x;

        float leftTorque = -leftForce * width / 2f;
        float rightTorque = rightForce * width / 2f;

        rigidbody2D.AddTorque((rightTorque + leftTorque) * rotationTorqueMultiplier * (hasStern ? sternRotationMultiplier : 1f) * Time.fixedDeltaTime);
    }
}
