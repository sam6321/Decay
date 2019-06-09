using System;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Represents the cooldown of an action that can only happen at a certain frequency.
    /// Example usage
    /// <example>
    /// <code>
    /// private Cooldown cooldown = new Cooldown(0.1);
    /// void Update()
    /// {
    ///     if(cooldown.Check(Time.time)) 
    ///     {
    ///         DoAction();
    ///     }
    /// }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class Cooldown
    {
        [SerializeField]
        private float frequency;

        private float last = 0.0f;

        /// <summary>
        /// The frequency, in seconds between action, that the cooldown should expire at
        /// </summary>
        public float Frequency { get => frequency; set => frequency = value; }

        /// <summary>
        /// The last time the cooldown expired.
        /// This will be 0 if Check has not been called at least once
        /// </summary>
        public float Last => last;

        /// <summary>
        /// The next time the cooldown will expire
        /// </summary>
        public float Next => last + frequency;

        /// <summary>
        /// Create a new cooldown instance
        /// </summary>
        /// <param name="frequency">The initial / default frequency</param>
        public Cooldown(float frequency)
        {
            Frequency = frequency;
        }

        /// <summary>
        /// Get the current progress towards the next cooldown expiry as a value between 0 and 1
        /// </summary>
        /// <param name="time">The time to get the progress at</param>
        /// <returns>The current progress at the specified time</returns>
        public float GetProgress(float time)
        {
            return Mathf.InverseLerp(last, last + frequency, time);
        }

        /// <summary>
        /// Check if the cooldown has expired and re-set the timer if it has.
        /// </summary>
        /// <param name="time">The current time</param>
        /// <returns>True if the cooldown has expired and the action should be performed, false if not</returns>
        public bool Check(float time)
        {
            if (time >= Next)
            {
                last = time;
                return true;
            }
            return false;
        }
    }
}