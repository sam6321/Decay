using UnityEngine;

[CreateAssetMenu(fileName = "Ship Component", menuName = "Ship Component", order = 4)]
public class ShipComponent : ScriptableObject
{
    public string componentName;

    public GameObject[] prefabs;
    public uint maxSpawned;
    public float spawnRate;
}
