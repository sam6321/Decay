using System;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class FloatingComponentSpawner : MonoBehaviour
{
    public class SpawnTag
    {
        public FloatingComponentSpawner spawner;
        public SpawnInfo spawnInfo;
    }

    [Serializable]
    public class SpawnInfo
    {
        public uint maxSpawned = 0;
        public GameObject[] prefabs;
        public float spawnRate;

        [HideInInspector]
        public List<FloatingComponent> spawned = new List<FloatingComponent>();
        [HideInInspector]
        public float nextSpawn = 0;
    }

    [SerializeField]
    private SpawnInfo[] spawnInfos;

    void Update()
    {
        foreach(SpawnInfo spawnInfo in spawnInfos)
        {
            if (Time.time >= spawnInfo.nextSpawn && spawnInfo.spawned.Count < spawnInfo.maxSpawned)
            {
                GameObject prefab = RandomExtensions.RandomElement(spawnInfo.prefabs);
                // TODO: Set position properly so it doesn't spawn inside the camera's view
                GameObject instance = Instantiate(prefab, UnityEngine.Random.insideUnitCircle * 40, Quaternion.identity);
                FloatingComponent component = instance.GetComponent<FloatingComponent>();
                component.SpawnTag = new SpawnTag() { spawnInfo = spawnInfo};

                // Add as spawned by this spawner
                Add(component);

                spawnInfo.nextSpawn = Time.time + spawnInfo.spawnRate;
            }
        }
    }

    public void Add(FloatingComponent component)
    {
        component.SpawnTag.spawner = this;
        if(!component.SpawnTag.spawnInfo.spawned.Contains(component))
        {
            // Add if this spawner doesn't already own the component
            component.SpawnTag.spawnInfo.spawned.Add(component);
        }
    }

    public void Remove(FloatingComponent component)
    {
        component.SpawnTag.spawner = null;
        component.SpawnTag.spawnInfo.spawned.Remove(component); // Remove if added
    }

    public IEnumerable<FloatingComponent> FindNearbySpawnedComponents(ShipStructure structure, float distance) 
    {
        foreach(SpawnInfo info in spawnInfos)
        {
            foreach(FloatingComponent spawned in info.spawned)
            {
                if(spawned.DistanceTo(structure) <= distance)
                {
                    yield return spawned;
                }
            }
        }
    }
}
