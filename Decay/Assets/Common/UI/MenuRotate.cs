using UnityEngine;

namespace Common
{
    /// <summary>
    /// Rotates a set of transforms around the position of object that this behaviour is on.
    /// The objects move outward and inward over time and are placed evenly apart from each other
    /// </summary>
    public class MenuRotate : MonoBehaviour
    {
        /// <summary>
        /// The transforms to rotate
        /// </summary>
        [SerializeField]
        private Transform[] objects;

        /// <summary>
        /// Minimum distance the objects will be from the origin
        /// </summary>
        [SerializeField]
        private float offsetDistanceMin;

        /// <summary>
        /// Maximum distance the objects will be from the origin
        /// </summary>
        [SerializeField]
        private float offsetDistanceMax;

        /// <summary>
        /// Speed that the objects move between offsetDistanceMin and offsetDistanceMax
        /// </summary>
        [SerializeField]
        private float offsetPulseSpeed;

        /// <summary>
        /// Speed that the transforms rotate around the origin
        /// </summary>
        [SerializeField]
        private float rotateSpeed;

        void Update()
        {
            float rotationPortion = 360.0f / objects.Length;

            for (int i = 0; i < objects.Length; i++)
            {
                float factor = Mathf.Cos(Time.time * offsetPulseSpeed) * 0.5f + 0.5f;
                float distance = Mathf.Lerp(offsetDistanceMin, offsetDistanceMax, factor);
                Vector3 offset = Quaternion.AngleAxis(rotationPortion * i + Time.time * rotateSpeed, Vector3.forward) * new Vector3(distance, 0, 0);
                objects[i].position = transform.position + offset;
            }
        }
    }
}
