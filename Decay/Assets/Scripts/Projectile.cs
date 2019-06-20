using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private ShipStructure source;
    public ShipStructure Source { get => source; set => source = value; }

    [SerializeField]
    private float damage = 1;

    [SerializeField]
    private GameObject onImpactParticles;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Calculate the damage we should take on collision
        ShipStructure structure = collider.GetComponentInParent<ShipStructure>();
        if(structure)
        {
            if(structure == source)
            {
                return;
            }

            structure.ApplyDamage(collider, new DamageInfo()
            {
                source = source,
                amount = damage,
                force = (collider.transform.position - transform.position).normalized * 100
            });
        }

        Instantiate(onImpactParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
