using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab; // Assign in inspector
    [SerializeField] private float spawnRate = 3.0f; // Time between spawns
    [SerializeField] private Transform[] spawnPoints; // Assign spawn points in inspector

    private float nextSpawnTime;

    [SerializeField] private Material[] materials;
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
        // Randomize material for each child
        foreach (Transform child in obstacle.transform)
        {
            var renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = materials[Random.Range(0, materials.Length)];
            }
        }

        obstacle.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("End"))
        {
            
            Destroy(gameObject); // Destroy the obstacle
        }
    }
}
