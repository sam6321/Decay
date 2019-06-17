using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipManager : MonoBehaviour
{
    [SerializeField]
    private List<ShipStructure> ships = new List<ShipStructure>();
    public IReadOnlyList<ShipStructure> Ships => ships;

    [SerializeField]
    private ShipStructure player;

    public void RemoveShip(ShipStructure structure)
    {
        ships.Remove(structure);
        if(structure == player)
        {
            Lose();
        }
        else if(ships.Count == 1)
        {
            Win();
        }
    }

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

    private void Win()
    {

    }

    private void Lose()
    {

    }
}
