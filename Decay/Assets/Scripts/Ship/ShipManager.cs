using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Common;

public class ShipManager : MonoBehaviour
{
    [System.Serializable]
    public class OnShipDestroyEvent : UnityEvent<ShipStructure, ShipManager> { }

    [SerializeField]
    private GameObject npcPrefab;

    [SerializeField]
    private uint npcsToSpawn;

    [SerializeField]
    private Bounds spawnVolume = new Bounds();
    public Bounds SpawnVolume => spawnVolume;

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

    private void Start()
    {
        for(uint i = 0; i < npcsToSpawn; i++)
        {
            Vector2 position = RandomExtensions.RandomInsideBounds(spawnVolume);
            GameObject spawned = Instantiate(npcPrefab, position, Quaternion.identity);

            ShipStructure structure = spawned.GetComponent<ShipStructure>();
            structure.OnLose.AddListener(RemoveShip);
            ships.Add(structure);
        }
    }

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
