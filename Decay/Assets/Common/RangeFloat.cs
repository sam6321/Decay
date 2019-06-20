using System;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Stores a pair of values representing the start and end of a range.
    /// The pair can be set via the Unity inspector.
    /// </summary>
    [Serializable]
    public class RangeFloat
    {
        /// <summary>
        /// Start range value. Should be less than the end range value.
        /// </summary>
        public float start;

        /// <summary>
        /// End range value. Should be greater than the start range value.
        /// </summary>
        public float end;

        /// <summary>
        /// Construct a range pair
        /// </summary>
        /// <param name="start">Start range value</param>
        /// <param name="end">End range value</param>
        public RangeFloat(float start, float end)
        {
            this.start = start;
            this.end = end;
        }

        /// <summary>
        /// Clamp a value to the range
        /// </summary>
        /// <param name="value">Value to clamp</param>
        /// <returns>Value clamped to the range</returns>
        public float Clamp(float value)
        {
            return Mathf.Clamp(value, start, end);
        }

        /// <summary>
        /// Get a value between the start and end, determined by a factor between 0 and 1
        /// </summary>
        /// <param name="factor">The factor, between 0 and 1</param>
        /// <returns>The value between the start and end at the given factor</returns>
        public float InverseLerp(float factor)
        {
            return Mathf.InverseLerp(start, end, factor);
        }

        /// <summary>
        /// Get a random value between the range values
        /// </summary>
        /// <returns>The random value</returns>
        public float Random()
        {
            return UnityEngine.Random.Range(start, end);
        }
    }
}

