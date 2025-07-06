using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour
{
    public GameObject playerToSpawn;
    public Vector3 spawnPosition;
    public bool gameReady;
    public bool hasSpawned;

    public GameObject spawnEffect;

    void Awake()
    {
        spawnPosition = gameObject.transform.position;
    }

    void Update()
    {

    }

    public void Spawn()
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
