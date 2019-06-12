using UnityEngine;

public class PositionFollow : MonoBehaviour
{
    [SerializeField]
    private float smoothing = 5.0f;

    [SerializeField]
    private float distance = 0.5f;

    [SerializeField]
    private Transform target;
    public Transform Target { get => target;  set => target = value; }

    private Vector2 lastTarget = Vector2.zero;
    private Vector2 offset = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        if(target)
        {
            if(Vector2.Distance(lastTarget, target.position) > 0)
            {
                offset = (lastTarget - (Vector2)target.position).normalized * distance;
            }

            transform.position = Vector2.Lerp(transform.position, (Vector2)target.position + offset, smoothing * Time.deltaTime);
            lastTarget = transform.position;
        }
    }
}
