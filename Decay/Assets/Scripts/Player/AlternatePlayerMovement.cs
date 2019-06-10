using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class AlternatePlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        float hor = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        if(vert != 0.0f){
            rigidbody2D.AddRelativeForce(new Vector2(0.0f, 5.0f * vert));
        }
        else{
            rigidbody2D.AddRelativeForce(new Vector2(0.0f, 2.0f * Mathf.Abs(hor)));
        }
        
        if(rigidbody2D.angularVelocity < 50.0f && hor < 0.0f){
            rigidbody2D.AddTorque(-1.5f * hor);
        }
        else if(rigidbody2D.angularVelocity > -50.0f && hor > 0.0f){
            rigidbody2D.AddTorque(-1.5f * hor);
        }
    }
}
