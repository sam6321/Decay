using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oar : ShipComponent
{
    protected override bool DoAttach(ShipStructure structure)
    {
        return structure.AddOar(this);
    }

    protected override bool DoDetach(ShipStructure structure)
    {
        return structure.RemoveOar(this);
    }
}
