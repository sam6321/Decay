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
        public bool ownedBySpawner;
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
    private Bounds spawnBounds = new Bounds();

    [SerializeField]
    private SpawnInfo[] spawnInfos;

    void LateUpdate()
    {
        foreach(SpawnInfo spawnInfo in spawnInfos)
        {
            if (Time.time >= spawnInfo.nextSpawn && spawnInfo.spawned.Count < spawnInfo.maxSpawned)
            {
                GameObject prefab = RandomExtensions.RandomElement(spawnInfo.prefabs);
                // TODO: Set position properly so it doesn't spawn inside the camera's view
                Vector2 position = RandomExtensions.RandomInsideBounds(spawnBounds);
                GameObject instance = Instantiate(prefab, position, Quaternion.identity);
                FloatingComponent component = instance.GetComponent<FloatingComponent>();
                component.SpawnTag = new SpawnTag() { spawnInfo = spawnInfo, spawner = this };

                // Note, after instantiating this object, start won't be called until next frame. To prevent other objects from running
                // into trouble trying to use a non-initialised floating component, the script execution of the spawner is set to be after
                // everything else.

                // Add as spawned by this spawner
                Add(component);

                spawnInfo.nextSpawn = Time.time + spawnInfo.spawnRate;
            }
        }
    }

    public void Add(FloatingComponent component)
    {
        component.SpawnTag.ownedBySpawner = true;
        if(!component.SpawnTag.spawnInfo.spawned.Contains(component))
        {
            // Add if this spawner doesn't already own the component
            component.SpawnTag.spawnInfo.spawned.Add(component);
        }
    }

    public void Remove(FloatingComponent component)
    {
        component.SpawnTag.ownedBySpawner = false;
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
