using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class PowerUpManager : MonoBehaviour {

    /// <summary>
    /// The initial spawn delay of the starting power ups
    /// </summary>
    [SerializeField]
    private float initialSpawnDelay = 5f;

    /// <summary>
    /// The inital amount of powerups.
    /// </summary>
    [SerializeField]
    private int initalAmountOfPowerups = 6;

    /// <summary>
    /// The spawn delay of each power up
    /// </summary>
    [SerializeField]
    private float spawnDelay = 10f;

    [SerializeField]
    private List<GameObject> spawnPoints;

    private float spawnTimer;

    private PoolManager poolManager;

    /// <summary>
    /// Awake this instance.
    /// </summary>
    void Awake() {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("PowerUpSpawnPoints")) {
            item.AddComponent<PowerUpSpawnPoint>();
            spawnPoints.Add(item);
        }

        spawnTimer = Time.time + spawnDelay;
        poolManager = PoolManager.instance;
    }

    /// <summary>
    /// Update this instance.
    /// </summary>
    void Update() {
        if (spawnTimer < Time.time) {
            spawnPowerUp();
            spawnTimer = Time.time + spawnDelay;
        }
    }

    /// <summary>
    /// Spawns a power up.
    /// </summary>
    private void spawnPowerUp() {
        GameObject randomSpawnPoint = getRandomSpawnPoint();
        if (randomSpawnPoint == null) {
            Debug.Log("All power up spawn points are allocated. Can't spawn new power up.");
            return;
        }
        // Indexes of the powerups in the PoolInitialiser
        int randomPowerUp = UnityEngine.Random.Range(6, 9);
        GameObject powerUpPrefab = PoolInitialiser.instance.getManagedPrefab(randomPowerUp);
        Transform randomSpawnPointTransform = randomSpawnPoint.transform;

        PoolObject poolObject = poolManager.reuseObject(powerUpPrefab.GetInstanceID(), randomSpawnPointTransform, randomSpawnPointTransform.position, randomSpawnPointTransform.rotation);
        if (poolObject == null) {
            Debug.LogWarning("No power up spawned. Pool manager does not contain any " + powerUpPrefab.name + " objects");
            return;
        }

        Debug.Log("Spawned " + powerUpPrefab.name + " power up at spawn point " + randomSpawnPointTransform.name + " in location " + randomSpawnPointTransform.position);
    }

    /// <summary>
    /// Gets a spawn point for the power up.
    /// </summary>
    /// <returns>A random spawn point.</returns>
    private GameObject getRandomSpawnPoint() {
        int tries = 0;
        do {
            GameObject spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
            PowerUpSpawnPoint powerUpSpawnPoint = spawnPoint.GetComponent<PowerUpSpawnPoint>();
            if (false == powerUpSpawnPoint.HasPowerUp) {
                powerUpSpawnPoint.HasPowerUp = true;
                return spawnPoint;
            }
            tries++;
        } while (tries <= 5);
        return null;
    }
}
