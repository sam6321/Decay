using System;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class FloatingComponentSpawner : MonoBehaviour
{
    class SpawnInfo
    {
        public List<GameObject> spawned = new List<GameObject>();
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
                GameObject prefab = RandomExtensions.RandomElement(component.floatingPrefabs);
                GameObject instance = Instantiate(prefab);

                // TODO: Set position properly
                instance.transform.position = UnityEngine.Random.insideUnitCircle * 20;
                // Mark it with a destroy event listener so we can clear it from our internal lists on destroy
                instance.GetComponent<FloatingComponent>().OnComponentPickup.AddListener(OnSpawnedComponentDestroyed);

                spawnInfo.spawned.Add(instance);
                spawnInfo.nextSpawn = Time.time + component.spawnRate;
            }
        }
    }

    void OnSpawnedComponentDestroyed(FloatingComponent component)
    {
        int componentIndex = Array.IndexOf(components, component.ShipComponent);
        Debug.Assert(componentIndex >= 0, "Destroyed FloatingComponent has bad ShipComponent");
        SpawnInfo info = spawnInfos[componentIndex];
        info.spawned.Remove(component.gameObject);

        Debug.Log("Post-Destroy count for " + component.ShipComponent.componentName + ": " + info.spawned.Count);
    }
}
