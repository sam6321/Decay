using System;
using System.Linq;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// A single weighted random entry. 
    /// Declare an array of these to provide weighted random entries via the Unity Inspector
    /// </summary>
    /// <typeparam name="T">The type of object that this entry holds</typeparam>
    [Serializable]
    public class WeightedRandomEntry<T>
    {
        /// <summary>
        /// The item to spawn if this entry is selected
        /// </summary>
        public T item;

        /// <summary>
        /// The weight of this item
        /// </summary>
        public int weight;
    }

    /// <summary>
    /// Perform weighted random selection over a set of weighted random entries
    /// </summary>
    /// <typeparam name="T">The type of the items to select from</typeparam>
    class WeightedRandom<T>
    {
        private int totalWeight = 0;
        private WeightedRandomEntry<T>[] items;

        /// <summary>
        /// Construct a weighted random selector
        /// </summary>
        /// <param name="items">The weighted random items to select from</param>
        public WeightedRandom(params WeightedRandomEntry<T>[] items)
        {
            totalWeight = items.Sum(item => item.weight);
            this.items = items;
        }

        /// <summary>
        /// Get an item from the weighted random entries
        /// </summary>
        /// <returns>The chosen item</returns>
        public T GetItem()
        {
            int weight = UnityEngine.Random.Range(0, totalWeight);
            foreach (WeightedRandomEntry<T> entry in items)
            {
                if (weight < entry.weight)
                {
                    return entry.item;
                }
                weight -= entry.weight;
            }
            Debug.Log("Should not be here");
            return items[0].item;
        }

        /// <summary>
        /// Get several items from the random entries
        /// </summary>
        /// <param name="count">The number of items to get</param>
        /// <returns>Array containing the randomly selected items</returns>
        public T[] GetItems(uint count)
        {
            T[] items = new T[count];
            for (uint i = 0; i < count; i++)
            {
                items[i] = GetItem();
            }
            return items;
        }
    }
}
