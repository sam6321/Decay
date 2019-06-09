using UnityEngine;

namespace Common
{
    /// <summary>
    /// Rotates the attached object steadily over time
    /// </summary>
    public class Rotate : MonoBehaviour
    {
        /// <summary>
        /// The rotation speed, in degrees per second
        /// </summary>
        [SerializeField]
        private float rotateSpeed;

        void Update()
        {
            transform.eulerAngles = new Vector3(0, 0, rotateSpeed * Time.time);
        }
    }
}
