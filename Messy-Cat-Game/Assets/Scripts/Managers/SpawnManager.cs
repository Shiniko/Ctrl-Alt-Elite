using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float messageDelay;

    public SpawnPlayer starterSpawn;
    public SpawnPlayer lastSpawnPoint;
    public GameObject playerToSpawn;
    [SerializeField] private SpawnPlayer[] playerSpawners;
    [SerializeField] private bool hasSetSpawners;

    [SerializeField] private GameObject ghost;
    [SerializeField] private GameObject announceUI;

    public int lastPlayerWaypoint;

    void Start()
    {
        playerSpawners = gameObject.GetComponentsInChildren<SpawnPlayer>();

        if (gameManager == null)
        {
            if (GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>() != null)
            {
                gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            }
        }
    }

    void Update()
    {
        if (!hasSetSpawners)
        {
            if (gameManager != null)
            {
                if (gameManager.gameReady)
                {
                    hasSetSpawners = true;

                    SetSpawners();
                }
            }
        }
    }

    public void SpawnPlayer()
    {
        if (lastSpawnPoint != null)
        {
            lastSpawnPoint.Spawn();
        }
        else
        {
            if (starterSpawn != null)
            {
                starterSpawn.Spawn();
            }
        }

        if (ghost != null)
        {
            if (ghost.GetComponent<FollowPlayer>() != null)
            {
                ghost.GetComponent<FollowPlayer>().spawnOffset = true;
            }
        }
    }

    public void SetSpawners()
    {
        for(int i = 0; i < playerSpawners.Length; i++)
        {
            playerSpawners[i].playerToSpawn = playerToSpawn;

            //Debug.Log("Setting player to Spawn for " + i + " to " + playerToSpawn);
        }
    }

    public void ChangeSpawnPoint(SpawnPlayer spawner)
    {
        Debug.Log("Called change spawn point");

        if (spawner.GetComponent<SpawnPlayer>() != null)
        {
            lastSpawnPoint = spawner.GetComponent<SpawnPlayer>();

            //Debug.Log("Changed Spawn point");

            for (int i = 0; i < playerSpawners.Length; i++)
            {
                if (playerSpawners[i] == spawner)
                {
                    lastPlayerWaypoint = i;
                }
            }

            if (announceUI != null)
            {
                announceUI.SetActive(true);

                StartCoroutine(DelayCloseAnnouncements(messageDelay));
            }

            int wpCheck = PlayerPrefs.GetInt("LastWayPoint");

            if (wpCheck != lastPlayerWaypoint)
            {
                PlayerPrefs.SetInt("LastWayPoint", lastPlayerWaypoint);

                Debug.Log("Changed last waypoint to " + lastPlayerWaypoint);

                if (gameManager != null)
                {
                    gameManager.lastPlayerWaypoint = lastPlayerWaypoint;
                }
            }
        }
    }

    public void SetInitialSpawner(int num)
    {
        if(num >= 0 && num < playerSpawners.Length)
        {
            lastPlayerWaypoint = num;

            lastSpawnPoint = playerSpawners[num];
        }
    }

    IEnumerator DelayCloseAnnouncements(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (announceUI != null)
        {
            announceUI.SetActive(false);
        }
    }
}
