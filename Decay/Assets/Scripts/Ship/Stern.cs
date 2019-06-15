using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stern : ShipComponent
{
    [SerializeField]
    private Transform rudder;

    private Rigidbody2D structureRigidBody;

    void Update()
    {
        if(structureRigidBody)
        {
            rudder.localRotation = Quaternion.AngleAxis(-structureRigidBody.angularVelocity, Vector3.forward);
        }
    }

    protected override bool DoAttach(ShipStructure structure)
    {
        if(structure.AddStern(this))
        {
            structureRigidBody = structure.GetComponent<Rigidbody2D>();
            return true;
        }
        return false;
    }

    protected override bool DoDetach(ShipStructure structure)
    {
        if(structure.RemoveStern(this))
        {
            structureRigidBody = null;
            return true;
        }
        return false;
    }
}
