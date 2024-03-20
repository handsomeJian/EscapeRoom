using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab; // Assign in inspector
    [SerializeField] private float spawnRate = 3.0f; // Time between spawns
    [SerializeField] private Transform[] spawnPoints; // Assign spawn points in inspector
    [SerializeField] private Animator JJlevel2Anim;

    private float nextSpawnTime;

    [SerializeField] private Sprite[] sprites;
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            PlayAttackAnim();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    void SpawnObstacle()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        var obstacle = Instantiate(obstaclePrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);

        // Randomize sprite for each child
        foreach (Transform child in obstacle.transform)
        {
            var spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
            }
        }

        obstacle.SetActive(true);
    }
    void PlayAttackAnim()
    {
        JJlevel2Anim.SetBool("isAttack", true);
        StartCoroutine(PlayIdle(1f));
    }
    IEnumerator PlayIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        JJlevel2Anim.SetBool("isAttack", false);
    }
}
