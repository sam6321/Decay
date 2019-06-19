using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Misc random functions, such as list shuffling and random element selection
    /// </summary>
    static class RandomExtensions
    {
        /// <summary>
        /// Shuffle an array of any type in place
        /// </summary>
        /// <typeparam name="T">The array type</typeparam>
        /// <param name="array">The array to shuffle</param>
        /// <returns>The array</returns>
        public static T[] Shuffle<T>(T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                T tmp = array[i];
                int index = UnityEngine.Random.Range(0, array.Length);
                array[i] = array[index];
                array[index] = tmp;
            }

            return array;
        }

        /// <summary>
        /// Pick a random element from a list
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the list is empty</exception>
        /// <typeparam name="T">The list type</typeparam>
        /// <param name="list">The list to get the element from</param>
        /// <returns>The random element</returns>
        public static T RandomElement<T>(IList<T> list)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Picking random from empty list");
            }
            else
            {
                return list[UnityEngine.Random.Range(0, list.Count)];
            }
        }

        /// <summary>
        /// Picks multiple random elements from the list
        /// </summary>
        /// <typeparam name="T">The type of the list</typeparam>
        /// <param name="list">The list to get elements from. If the list is empty, an empty array of length 0 is returned</param>
        /// <param name="numElements">The number of elements to take. If the list is smaller than this number, all elements will be returned. If this number is less than or equal to 0, an empty array of length 0 is returned</param>
        /// <returns>The selected elements</returns>
        public static T[] RandomElements<T>(IList<T> list, int numElements)
        {
            T[] shuffledElements = Shuffle(list.ToArray());
            return shuffledElements.Take(numElements).ToArray();
        }

        /// <summary>
        /// Plays a random one shot sound from the provided list on the provided audio source
        /// </summary>
        /// <param name="source">The audio source to play the sound on</param>
        /// <param name="clips">The clips to choose from</param>
        /// <returns>The audio clip that was played, or null if the clips list was empty</returns>
        public static AudioClip PlayRandomOneShot(AudioSource source, IList<AudioClip> clips)
        {
            try
            {
                AudioClip clip = RandomElement(clips);
                source.PlayOneShot(clip);
                return clip;
            }
            catch(InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Get a random point inside the bounds
        /// </summary>
        /// <param name="bounds">Bounds to get a point inside of</param>
        /// <returns>Random point inside of the bounds</returns>
        public static Vector3 RandomInsideBounds(Bounds bounds)
        {
            Vector3 vector = bounds.extents;
            vector.x *= UnityEngine.Random.Range(-1f, 1f);
            vector.y *= UnityEngine.Random.Range(-1f, 1f);
            vector.z *= UnityEngine.Random.Range(-1f, 1f);
            return vector + bounds.center;
        }
    }
}
