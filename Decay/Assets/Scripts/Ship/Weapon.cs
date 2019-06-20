using UnityEngine;
using Common;

public class Weapon : ShipComponent
{
    [SerializeField]
    private uint maxAmmo;

    [SerializeField]
    private uint currentAmmo;

    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private Vector2 shootOffset = new Vector2(0, 0);

    [SerializeField]
    private FixedCooldown fireCooldown = new FixedCooldown(0.5f);

    private Vector2 aimDirection = new Vector2();

    public void Aim(Vector2 target)
    {
        aimDirection = (target - (Vector2)transform.position).normalized;
        float angle = Vector2.SignedAngle(Vector2.up, aimDirection);
        transform.eulerAngles = new Vector3(0, 0, angle);
    }

    public bool Fire()
    {
        if(currentAmmo > 0 && fireCooldown.Check(Time.time))
        {
            GameObject projectile = Instantiate(projectilePrefab, (Vector2)transform.position + shootOffset, transform.rotation);
            projectile.GetComponent<Rigidbody2D>().AddForce(aimDirection * 1000);
            projectile.GetComponent<Projectile>().Source = attachedStructure;
            currentAmmo--;

            if(currentAmmo == 0)
            {
                // No more ammo, so explode off the ship's desk
                GetComponent<FloatingComponent>().TryReturnToSpawner();
            }

            return true;
        }
        return false;
    }

    protected override bool DoAttach(ShipStructure structure)
    {
        currentAmmo = maxAmmo;
        return structure.AddWeapon(this);
    }

    protected override bool DoDetach(ShipStructure structure)
    {
        return structure.RemoveWeapon(this);
    }
}
