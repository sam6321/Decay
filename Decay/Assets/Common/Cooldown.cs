using System;
using UnityEngine;

namespace Common
{
    public abstract class Cooldown
    {
        private float last = 0.0f;

        /// <summary>
        /// The last time the cooldown expired.
        /// This will be 0 if Check has not been called at least once
        /// </summary>
        public float Last => last;

        private float next = 0.0f;

        /// <summary>
        /// The next time the cooldown will expire
        /// </summary>
        public float Next => next;

        /// <summary>
        /// Get the current progress towards the next cooldown expiry as a value between 0 and 1
        /// </summary>
        /// <param name="time">The time to get the progress at</param>
        /// <returns>The current progress at the specified time</returns>
        public float GetProgress(float time)
        {
            return Mathf.InverseLerp(last, next, time);
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
                next = GetNext(time);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Reset the cooldown to begin at the specified time, without changing the next time the cooldown will
        /// expire.
        /// </summary>
        /// <param name="time">The time to set to</param>
        public void Reset(float time)
        {
            float diff = Math.Max(next - last, 0);
            last = time;
            next = last + diff;
        }

        protected abstract float GetNext(float time);
    }

    /// <summary>
    /// Represents the cooldown of an action that can only happen at a fixed frequency.
    /// Example usage
    /// <example>
    /// <code>
    /// private FixedCooldown cooldown = new FixedCooldown(0.1);
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
    public class FixedCooldown : Cooldown
    {
        [SerializeField]
        private float frequency;

        /// <summary>
        /// The frequency, in seconds between action, that the cooldown should expire at
        /// </summary>
        public float Frequency { get => frequency; set => frequency = value; }

        /// <summary>
        /// Create a new FixedCooldown instance
        /// </summary>
        /// <param name="frequency">The initial / default frequency</param>
        public FixedCooldown(float frequency)
        {
            Frequency = frequency;
        }

        protected override float GetNext(float time)
        {
            return time + frequency;
        }
    }

    /// <summary>
    /// Represents a cooldown that picks a random value within a range
    /// Example usage
    /// <example>
    /// <code>
    /// private RangeCooldown cooldown = new RangeCooldown(0.5, 1.0);
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
    public class RangeCooldown : Cooldown
    {
        [SerializeField]
        private RangeFloat range = new RangeFloat(0, 0);

        /// <summary>
        /// The range that that the cooldown is using, start and end defined in seconds.
        /// </summary>
        public RangeFloat Range => range;

        /// <summary>
        /// Construcr a new FixedCooldown instance
        /// </summary>
        /// <param name="start">The start of the range</param>
        /// <param name="end">The end of the range</param>
        public RangeCooldown(float start, float end)
        {
            range.start = start;
            range.end = end;
        }

        protected override float GetNext(float time)
        {
            return time + range.Random();
        }
    }
}