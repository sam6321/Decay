using System;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class FloatingComponentSpawner : MonoBehaviour
{
    class SpawnInfo
    {
        public List<FloatingComponent> spawned = new List<FloatingComponent>();
        public float nextSpawn = 0;
    }

    [SerializeField]
    private ShipComponent[] components;
    private SpawnInfo[] spawnInfos;

    void Start()
    {
        spawnInfos = new SpawnInfo[components.Length];
        for(int i = 0; i < spawnInfos.Length; i++)
        {
            spawnInfos[i] = new SpawnInfo();
        }
    }

    void Update()
    {
        for(int i = 0; i < components.Length; i++)
        {
            ShipComponent component = components[i];
            SpawnInfo spawnInfo = spawnInfos[i];

            if(Time.time >= spawnInfo.nextSpawn && spawnInfo.spawned.Count < component.maxSpawned)
            {
                GameObject prefab = RandomExtensions.RandomElement(component.prefabs);
                // TODO: Set position properly so it doesn't spawn inside the camera's view
                GameObject instance = Instantiate(prefab, UnityEngine.Random.insideUnitCircle * 20, Quaternion.identity);

                // Add as spawned by this spawner
                Add(instance.GetComponent<FloatingComponent>());

                spawnInfo.nextSpawn = Time.time + component.spawnRate;
            }
        }
    }

    public void Add(FloatingComponent component)
    {
        component.Spawner = this;
        SpawnInfo info = GetSpawnInfo(component.ShipComponent);
        if(!info.spawned.Contains(component))
        {
            // Add if this spawner doesn't already own the component
            info.spawned.Add(component);
        }
    }

    public void Remove(FloatingComponent component)
    {
        component.Spawner = null;
        SpawnInfo info = GetSpawnInfo(component.ShipComponent);
        info.spawned.Remove(component); // Remove if added
    }

    private SpawnInfo GetSpawnInfo(ShipComponent shipComponent)
    {
        return spawnInfos[Array.IndexOf(components, shipComponent)];
    }
}
