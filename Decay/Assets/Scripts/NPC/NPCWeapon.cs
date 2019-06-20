using System.Linq;
using UnityEngine;
using Common;

public class NPCWeapon : MonoBehaviour
{
    [SerializeField]
    private FixedCooldown acquireTargetCooldown = new FixedCooldown(1f);

    [SerializeField]
    private FixedCooldown loseTargetCooldown = new FixedCooldown(1f);

    [SerializeField]
    private float maxAcquireDistance = 10;

    private ShipStructure currentTarget;
    private ShipStructure structure;
    private ShipManager shipManager;

    // Start is called before the first frame update
    void Start()
    {
        structure = GetComponent<ShipStructure>();
        shipManager = GameObject.Find("GameManager").GetComponent<ShipManager>();
        shipManager.OnShipDestroyed.AddListener(OnShipDestroyed);
    }

    void OnDestroy()
    {
        shipManager.OnShipDestroyed.RemoveListener(OnShipDestroyed);
    }

    void OnShipDestroyed(ShipStructure structure, ShipManager manager)
    {
        // Null out the target if it gets destroyed
        if(currentTarget == structure)
        {
            currentTarget = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(structure.Weapon)
        {
            if(acquireTargetCooldown.Check(Time.time))
            {
                ShipStructure[] potentialTargets = shipManager.FindNearbyShips(structure, maxAcquireDistance).ToArray();
                if(potentialTargets.Length > 0)
                {
                    currentTarget = RandomExtensions.RandomElement(potentialTargets);
                    loseTargetCooldown.Reset(Time.time);
                } 
            }

            if(currentTarget && loseTargetCooldown.Check(Time.time) && structure.DistanceTo(currentTarget) > maxAcquireDistance)
            {
                currentTarget = null;
            }

            if(currentTarget)
            {
                structure.Weapon.Aim(currentTarget.transform.position);
                structure.Weapon.Fire();
            }
        }
    }
}
