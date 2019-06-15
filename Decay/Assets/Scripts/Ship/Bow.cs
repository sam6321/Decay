using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : ShipComponent
{
    protected override bool DoAttach(ShipStructure structure)
    {
        return structure.AddBow(this);
    }

    protected override bool DoDetach(ShipStructure structure)
    {
        return structure.RemoveBow(this);
    }
}
