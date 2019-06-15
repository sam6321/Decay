using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField]
    private List<ShipStructure> ships = new List<ShipStructure>();

    public IEnumerable<ShipStructure> FindNearbyShips(ShipStructure structure, float distance)
    {
        foreach(ShipStructure ship in ships)
        {
            if(structure.DistanceTo(ship) <= distance)
            {
                yield return ship;
            }
        }
    }
}
