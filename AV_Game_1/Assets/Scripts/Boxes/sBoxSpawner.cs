using UnityEngine;
using System.Collections.Generic;

public class sBoxSpawner : MonoBehaviour
{
    [System.Serializable]
    public class BoxSpawnEntry
    {
        public SO_BoxData boxData;
        public Transform spawnPoint;
    }

    [Tooltip("Boxes to spawn automatically on Start. Leave empty if you only want to spawn via code (e.g. from a level manager).")]
    public List<BoxSpawnEntry> boxesToSpawn;

    void Start()
    {
        //SpawnAll();
    }

    public void SpawnAll()
    {
        foreach (var entry in boxesToSpawn)
        {
            if (entry.spawnPoint == null)
            {
                Debug.LogWarning($"sBoxSpawner: No spawn point set for {entry.boxData?.boxName}, skipping.");
                continue;
            }

            SpawnBox(entry.boxData, entry.spawnPoint.position, entry.spawnPoint.rotation);
        }
    }

    public sBox SpawnBox(SO_BoxData data, Vector3 position, Quaternion rotation)
    {
        if (data == null || data.boxPrefab == null)
        {
            Debug.LogWarning("sBoxSpawner: Missing box data or prefab reference.");
            return null;
        }

        GameObject boxObj = Instantiate(data.boxPrefab, position, rotation);
        sBox box = boxObj.GetComponent<sBox>();

        if (box == null)
        {
            Debug.LogWarning($"sBoxSpawner: Spawned prefab for {data.boxName} has no sBox component.");
            return null;
        }

        box.Initialize(data);
        return box;
    }

    // Convenience overload if you just want position with default rotation
    public sBox SpawnBox(SO_BoxData data, Vector3 position)
    {
        return SpawnBox(data, position, Quaternion.identity);
    }
}
