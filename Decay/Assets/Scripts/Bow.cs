using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : ShipComponent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override bool DoAttach(ShipStructure structure)
    {
        return structure.AddBow(this);
    }

    protected override bool DoDetach(ShipStructure structure)
    {
        return structure.RemoveBow(this);
    }
}
