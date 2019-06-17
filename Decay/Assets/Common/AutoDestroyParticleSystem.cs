using UnityEngine;
using System.Collections;

namespace Common
{
    /// <summary>
    /// Automatically destroys the particle system on this object when it stops playing.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class AutoDestroyParticleSystem : MonoBehaviour
    {
        [SerializeField]
        private bool onlyDeactivate;

        private ParticleSystem particleSystem;

        private void Start()
        {
            particleSystem = GetComponent<ParticleSystem>();
            StartCoroutine(CheckIfAlive());
        }

        private IEnumerator CheckIfAlive()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.5f);
                if (!particleSystem.IsAlive(true))
                {
                    if (onlyDeactivate)
                    {
                        gameObject.SetActive(false);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                    break;
                }
            }
        }
    }
}