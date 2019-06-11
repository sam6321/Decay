using UnityEngine;

namespace Common
{
    /// <summary>
    /// Contains general math functions to extend the default <see cref="Mathf"/> ones
    /// </summary>
    static class MathExtensions
    {
        /// <summary>
        /// Clamp the magnitude of a value, regardless of its sign.
        /// </summary>
        /// <param name="value">The value to clamp</param>
        /// <param name="min">The minimum magnitude of the value. This should be positive</param>
        /// <param name="max">The maximum magnitude of the value. This should be positive</param>
        /// <returns>Magnitude clamped value</returns>
        public static float ClampMagnitude(float value, float min, float max)
        {
            return Mathf.Clamp(Mathf.Abs(value), min, max) * Mathf.Sign(value);
        }
    }
}
