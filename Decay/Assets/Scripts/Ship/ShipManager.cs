﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShipManager : MonoBehaviour
{
    [System.Serializable]
    public class OnShipDestroyEvent : UnityEvent<ShipStructure, ShipManager> { }

    [SerializeField]
    private GameObject onWinUI;

    [SerializeField]
    private GameObject onLoseUI;

    [SerializeField]
    private List<ShipStructure> ships = new List<ShipStructure>();
    public IReadOnlyList<ShipStructure> Ships => ships;

    [SerializeField]
    private ShipStructure player;

    [SerializeField]
    private OnShipDestroyEvent onShipDestroyed = new OnShipDestroyEvent();
    public OnShipDestroyEvent OnShipDestroyed => onShipDestroyed;

    public void RemoveShip(ShipStructure structure)
    {
        ships.Remove(structure);
        OnShipDestroyed.Invoke(structure, this);
        if (structure == player)
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
            if(ship != structure && structure.DistanceTo(ship) <= distance)
            {
                yield return ship;
            }
        }
    }

    public bool Exists(ShipStructure structure)
    {
        return ships.Contains(structure);
    }

    private void Win()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.enabled = false;
        onWinUI.SetActive(true);
    }

    private void Lose()
    {
        onLoseUI.SetActive(true);
    }
}
