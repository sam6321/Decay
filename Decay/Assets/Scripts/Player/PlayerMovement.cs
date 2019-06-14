using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float forwardMovementForce = 100f;

    [SerializeField]
    private float horizontalForwardMovementForce = 50f;

    [SerializeField]
    private float rotationTorqueMultiplier = 50.0f;

    private Rigidbody2D rigidbody2D;

    private Vector2 movement = new Vector2();

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }
    
    void FixedUpdate()
    {

        if(movement.y != 0)
        {
            float force = movement.y < 0 ? movement.y * 0.2f : movement.y;
            rigidbody2D.AddForce(transform.up * forwardMovementForce * force * Time.fixedDeltaTime);
        }
        else if(movement.x != 0)
        {
            rigidbody2D.AddForce(transform.up * horizontalForwardMovementForce * Mathf.Abs(movement.x) * Time.fixedDeltaTime);
        }
        
        if(movement.x != 0)
        {
            rigidbody2D.AddTorque(-movement.x * rotationTorqueMultiplier * Time.fixedDeltaTime);
        }
    }
}
