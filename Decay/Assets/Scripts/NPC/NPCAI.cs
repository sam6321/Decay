using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using Common.StateMachine;

public class NPCAI : MonoBehaviour
{
    enum States
    {
        Roam,
        PickUp,
        Attack,
        Flee
    }

    [SerializeField]
    private Cooldown itemCheckCooldown = new Cooldown(1.0f);

    [SerializeField]
    private float itemCheckDistance = 5.0f;

    [SerializeField]
    private float attackItemCheckDistance = 0.2f;

    [SerializeField]
    private Cooldown boatCheckCooldown = new Cooldown(1.0f);

    [SerializeField]
    private float boatCheckDistance = 5.0f;

    [SerializeField]
    private float attackGiveUpTimeout = 15.0f;

    [SerializeField]
    private float attackBackoffTimeout = 10.0f; // Don't attack again for 10 seconds after attacking

    [SerializeField]
    private int minPlanksToStartFight = 6;

    [SerializeField]
    private int minPlanksToContinueFight = 4;

    private FloatingComponentSpawner componentSpawner;
    private ShipManager shipManager;

    private NPCMovement movement;
    private ShipStructure shipStructure;
    private StateMachine<States> fsm;

    void Start()
    {
        GameObject gameManager = GameObject.Find("GameManager");
        componentSpawner = gameManager.GetComponent<FloatingComponentSpawner>();
        shipManager = gameManager.GetComponent<ShipManager>();
        shipManager.OnShipDestroyed.AddListener(OnShipDestroyed);

        movement = GetComponent<NPCMovement>();
        shipStructure = GetComponent<ShipStructure>();
        fsm = StateMachine<States>.Initialize(this);
        fsm.ChangeState(States.Roam);
    }

    void OnDestroy()
    {
        shipManager.OnShipDestroyed.RemoveListener(OnShipDestroyed);
    }

    void OnShipDestroyed(ShipStructure structure, ShipManager manager)
    {
        // Null out the target if it gets destroyed
        if (currentBoatTarget == structure)
        {
            currentBoatTarget = null;
        }
    }

    // Roam

    float nextItemCheck = 0.0f;
    float nextBoatCheck = 0.0f;

    void Roam_Enter()
    {

    }

    void Roam_Update()
    {
        // Pick random target to roam to, if no current target.
        if (!movement.MovementTarget.HasValue)
        {
            movement.MovementTarget = Random.insideUnitCircle * 40;
        }

        CheckForPickup(itemCheckDistance);

        // Check for nearby weak enemies, if any are found, transition to attack
        if(shipStructure.Planks.Count > minPlanksToStartFight &&
            boatCheckCooldown.Check(Time.time) && 
            (attackBackoffTime + attackBackoffTimeout <= Time.time) && // Don't attack again too soon, wait for the timeout first
            FindNearbyBoatTarget(out ShipStructure target))
        {
            currentBoatTarget = target;
            fsm.ChangeState(States.Attack);
        }
    }

    void Roam_Exit()
    {
        movement.MovementTarget = null;
    }

    void CheckForPickup(float maxDistance)
    {
        // Check for nearby items, if any are found, transition to pick up
        if(itemCheckCooldown.Check(Time.time) && 
            FindNearbyFloatingComponent(out FloatingComponent floatingComponent, maxDistance))
        {
            // Found something!
            currentPickupTarget = floatingComponent;
            fsm.ChangeState(States.PickUp);
        }
    }


    // PickUp

    private FloatingComponent currentPickupTarget;
    
    void PickUp_Enter()
    {
        movement.MovementTarget = currentPickupTarget.transform.position;
    }

    void PickUp_Update()
    {
        if(!currentPickupTarget.SpawnTag.spawner)
        {
            // Someone else picked it up, so just return to roaming
            fsm.ChangeState(fsm.LastState);
        }
        else if(currentPickupTarget.CanPickup(shipStructure))
        { 
            // Close enough to pick the item up!
            bool success = currentPickupTarget.TryAttachToShip(shipStructure);
            // If the pick up doesn't work, then that's fine, just go back to roaming
            fsm.ChangeState(fsm.LastState);
        }
    }

    void PickUp_Exit()
    {
        currentPickupTarget = null;
        movement.MovementTarget = null;
    }

    // Attack

    private ShipStructure currentBoatTarget;
    private float? attackStartTime = 0;
    private float attackBackoffTime = 0;

    void Attack_Enter()
    {
        if(attackStartTime == null)
        {
            attackStartTime = Time.time;
        }
    }

    void Attack_Update()
    {
        if(currentBoatTarget == null)
        {
            // Boat destroyed, go back to roaming
            fsm.ChangeState(States.Roam);
        }

        movement.MovementTarget = currentBoatTarget.transform.position;

        CheckForPickup(attackItemCheckDistance);

        // If chasing enemy for too long, return to roam
        if(shipStructure.Planks.Count < minPlanksToContinueFight || attackStartTime + attackGiveUpTimeout <= Time.time)
        {
            attackStartTime = null;
            fsm.ChangeState(States.Roam);
        }
    }

    void Attack_Exit()
    {
        movement.MovementTarget = null;
        attackBackoffTime = Time.time;
    }

    // Flee

    void Flee_Enter()
    {

    }

    void Flee_Update()
    {

    }

    void Flee_Exit()
    {

    }

    bool FindNearbyFloatingComponent(out FloatingComponent bestComponent, float maxDistance)
    {
        // TODO: don't go after a bow or stern if we don't have the planks for it
        bestComponent = null;
        foreach (FloatingComponent component in componentSpawner.FindNearbySpawnedComponents(shipStructure, maxDistance))
        {
            if(component.CompareTag("Bow") && !shipStructure.CanPickupBow)
            {
                continue;
            }

            if(component.CompareTag("Weapon") && !shipStructure.CanPickupWeapon)
            {
                continue;
            }

            if(component.CompareTag("Stern") && !shipStructure.CanPickupStern)
            {
                continue;
            }

            if(component.CompareTag("Oar") && !shipStructure.CanPickupOar)
            {
                continue;
            }

            if (!bestComponent)
            {
                bestComponent = component;
            }
            else if (component.NPCPriority > bestComponent.NPCPriority)
            {
                bestComponent = component;
            }
        }
        return bestComponent != null;
    }

    bool FindNearbyBoatTarget(out ShipStructure bestStructure)
    {
        bestStructure = null;
        foreach(ShipStructure structure in shipManager.FindNearbyShips(shipStructure, boatCheckDistance))
        {
            // TODO: Go after the weakest boat
            if(!bestStructure || bestStructure.Planks.Count <= structure.Planks.Count)
            {
                bestStructure = structure;
            }
        }
        return bestStructure != null;
    }
}
