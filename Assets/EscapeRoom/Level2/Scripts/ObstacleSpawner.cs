using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public GameObject obstaclePrefab; // Assign in inspector
    public float spawnRate = 3.0f; // Time between spawns
    public Transform[] spawnPoints; // Assign spawn points in inspector

    private float nextSpawnTime;

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    void SpawnObstacle()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        var obstacle= Instantiate(obstaclePrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        obstacle.SetActive(true);
    }
}
