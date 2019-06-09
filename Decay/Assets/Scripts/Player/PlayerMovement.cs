using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    void Start()
    {
        
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
        }
    }
}
