using UnityEngine;

namespace Common
{

    /// <summary>
    /// Implements a camera movement profile that smoothly moves to the target 
    /// over time
    /// </summary>
    public class CameraFollowMovement : MonoBehaviour
    {
        /// <summary>
        /// The transform to follow.
        /// This is preferred over <see cref="positionTarget"/> if both are set
        /// </summary>
        [SerializeField]
        private Transform transformTarget;

        /// <summary>
        /// Position target to move toward.
        /// <see cref="transformTarget"/> is preferred over this value if both are set
        /// </summary>
        [SerializeField]
        private Vector3? positionTarget;

        /// <summary>
        /// Smoothing factor for the camera movement
        /// </summary>
        [SerializeField]
        private float smoothing = 5.0f;

        public float Smoothing { get => smoothing; set => smoothing = value; }

        /// <summary>
        /// Final position offset that the camera's destination will be from the target
        /// </summary>
        [SerializeField]
        private Vector3 offset = new Vector3(0, 0, -10);

        /// <summary>
        /// Bounds to clamp the camera's edge bounds within.
        /// If these are set to all 0, no clamping will occur
        /// </summary>
        [SerializeField]
        private Bounds clampBounds = new Bounds();

        /// <summary>
        /// <see cref="transformTarget"/>
        /// </summary>
        public Transform TransformTarget { get => transformTarget; set => transformTarget = value; }

        /// <summary>
        /// <see cref="positionTarget"/>
        /// </summary>
        public Vector3? PositionTarget { get => positionTarget; set => positionTarget = value; }

        /// <summary>
        /// <see cref="clampBounds"/>
        /// </summary>
        public Bounds ClampBounds { get => clampBounds; set => clampBounds = value; }

        /// <summary>
        /// Get the current camera target.
        /// If the transform target is set, it will be returned, otherwise the position target is returned.
        /// </summary>
        /// <returns>Optional Vector3 which will contain a value if the transform target or position target are set, and will be empty otherwise</returns>
        public Vector3? GetCurrentTarget()
        {
            if (transformTarget != null)
            {
                return TransformTarget.position;
            }
            else
            {
                return PositionTarget;
            }
        }

        private Vector3 ClampPosition(Vector3 position)
        {
            if (clampBounds.min.x != 0 || clampBounds.min.y != 0 || clampBounds.max.x != 0 || clampBounds.max.y != 0)
            {
                float vertExtent = Camera.main.orthographicSize;  
                float horzExtent = vertExtent * Screen.width / Screen.height;
                float leftBound = clampBounds.min.x + horzExtent;
                float rightBound = clampBounds.max.x - horzExtent;
                float bottomBound = clampBounds.min.y + vertExtent;
                float topBound = clampBounds.max.y - vertExtent;
                position.x = Mathf.Clamp(position.x, leftBound, rightBound);
                position.y = Mathf.Clamp(position.y, bottomBound, topBound);
            }

            return position;
        }

        private void LateUpdate()
        {
            Vector3? target = GetCurrentTarget();
            if (target.HasValue)
            {
                Vector3 boundedTarget = ClampPosition(target.Value);
                transform.position = Vector3.Lerp(transform.position, boundedTarget + offset, smoothing * Time.deltaTime);
            }
        }
    }
}