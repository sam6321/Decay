using UnityEngine;

namespace Common
{
    /// <summary>
    /// Plays collision audio on impact with another object.
    /// Self collisions do not play sounds.
    /// Requires an AudioSource behaviour to be present on the object.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class CollisionAudio : MonoBehaviour
    {
        [SerializeField]
        private AudioGroup group;

        private AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            if (!collision.collider.transform.IsChildOf(transform))
            {
                group.PlayRandomOneShot(audioSource);
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if(!collision.collider.transform.IsChildOf(transform))
            {
                group.PlayRandomOneShot(audioSource);
            }
        }
    }
}
