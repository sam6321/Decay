using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// Contains a group of a certain type of asset.
    /// </summary>
    /// <typeparam name="T">The asset class type</typeparam>
    public class AssetGroup<T> : ScriptableObject, IReadOnlyList<T>
    {
        [SerializeField]
        protected T[] assets;

        /// <summary>
        /// Get a random asset from the asset group
        /// </summary>
        /// <returns>The random asset</returns>
        public T GetRandom()
        {
            return RandomExtensions.RandomElement(assets);
        }

        /// <summary>
        /// Selects an asset by index
        /// </summary>
        /// <param name="index">The index</param>
        /// <returns>The asset at the index</returns>
        public T this[int index] => ((IReadOnlyList<T>)assets)[index];

        /// <summary>
        /// Get the number of assets in the asset group
        /// </summary>
        public int Count => ((IReadOnlyList<T>)assets).Count;

        /// <summary>
        /// Get an enumerator over the assets
        /// </summary>
        /// <returns>Enumerator over the assets</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return ((IReadOnlyList<T>)assets).GetEnumerator();
        }

        /// <summary>
        /// Get an enumerator over the assets
        /// </summary>
        /// <returns>Enumerator over the assets</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IReadOnlyList<T>)assets).GetEnumerator();
        }
    }
}

