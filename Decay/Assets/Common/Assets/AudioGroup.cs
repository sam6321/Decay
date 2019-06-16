using UnityEngine;

namespace Common
{
    /// <summary>
    /// Represents a group of audio clips as a single asset.
    /// </summary>
    [CreateAssetMenu(fileName = "Audio Group", menuName = "Audio Group", order = 4)]
    public class AudioGroup : AssetGroup<AudioClip>
    {
        /// <summary>
        /// Play a one shot audio clip on the provided audio source
        /// </summary>
        /// <param name="source">The audio source to play the one shot on</param>
        /// <returns>The audio clip that was played</returns>
        public AudioClip PlayRandomOneShot(AudioSource source)
        {
            return RandomExtensions.PlayRandomOneShot(source, assets);
        }
    }
}
