using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerToSpawn;
    public Vector3 spawnPosition;
    public bool gameReady;
    public bool hasSpawned;
    public bool hasSetSpawnPosition;

    public GameObject spawnEffect;

    void Awake()
    {
        if (!hasSetSpawnPosition)
        {
            hasSetSpawnPosition = true;

            spawnPosition = gameObject.transform.position;
        }
    }

    void Update()
    {
        if (!hasSetSpawnPosition)
        {
            hasSetSpawnPosition = true;

            spawnPosition = gameObject.transform.position;
        }
    }

    public void Spawn()
    {
        if (hasSetSpawnPosition)
        {
            hasSpawned = true;

            if (playerToSpawn != null)
            {
                Instantiate(playerToSpawn, spawnPosition, Quaternion.identity);
            }

            if (spawnEffect != null)
            {
                Instantiate(spawnEffect, spawnPosition, Quaternion.identity);
            }
        }
    }
}
