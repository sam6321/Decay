using UnityEngine;

namespace Common
{
    /// <summary>
    /// Rotates an element around the Z axis gently between two rotations
    /// </summary>
    public class SoftRock : MonoBehaviour
    {
        /// <summary>
        /// The min Z rotation
        /// </summary>
        [SerializeField]
        private float minRot;

        /// <summary>
        /// The max Z rotation
        /// </summary>
        [SerializeField]
        private float maxRot;

        void Update()
        {
            float mod = (Mathf.Cos(Time.time) + 1) * 0.5f;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(minRot, maxRot, mod));
        }
    }
}