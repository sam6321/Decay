using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Rotate toward movement direction
        Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 direction = movement.normalized;
        if (direction.magnitude > 0)
        {
            float angle = Vector2.SignedAngle(Vector2.up, direction);
            angle = Mathf.MoveTowardsAngle(transform.eulerAngles.z, angle, 360 * Time.deltaTime);
            transform.eulerAngles = new Vector3(0, 0, angle);
            rigidbody2D.AddForce(direction * 10);
        }
    }
}
