using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plank : ShipComponent
{
    protected override bool DoAttach(ShipStructure structure)
    {
        return structure.AddPlank(this);
    }

    protected override bool DoDetach(ShipStructure structure)
    {
        return structure.RemovePlank(this);
    }
}
