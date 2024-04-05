using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstaclePrefab, computerfile; // Assign in inspector
    [SerializeField] private float spawnRate = 3.0f, animationTime = 0.08f; // Time between spawns
    [SerializeField] private Transform[] spawnPoints; // Assign spawn points in inspector
    [SerializeField] private Animator JJlevel2Anim;

    private float nextSpawnTime;
   // private Animation anim;

    [SerializeField] private Sprite[] sprites;
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            
            PlayAttackAnim();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //JJlevel2Anim.SetTrigger("takeDamage");
            Debug.Log("taking damage in anim");
        }
    }

    void SpawnObstacle( int randomIndex)
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        var obstacle = Instantiate(obstaclePrefab, spawnPoints[spawnIndex].position, spawnPoints[spawnIndex].rotation);
        obstacle.transform.localScale *= transform.parent.localScale.x;

        // Randomize sprite for each child
        foreach (Transform child in obstacle.transform)
        {
            var spriteRenderer = child.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = sprites[randomIndex];

            }
        }

        obstacle.SetActive(true);

    }
    void PlayAttackAnim()
    {
        //change animation sprite
        int randomIndex = Random.Range(0, sprites.Length);
        computerfile.GetComponent<SpriteRenderer>().sprite = sprites[randomIndex];
        if (JJlevel2Anim.gameObject.activeSelf == false)
        {
            JJlevel2Anim.gameObject.SetActive(true);
        }

        ;
        //JJlevel2Anim.SetBool("isAttack", true);
        JJlevel2Anim.SetTrigger("isAttacking");
        StartCoroutine(PlayIdle(animationTime, randomIndex));
    }
    IEnumerator PlayIdle(float delay, int randomIndex)
    {
        yield return new WaitForSeconds(delay);
        //JJlevel2Anim.SetBool("isAttack", false);
        //JJlevel2Anim.SetTrigger("Idle");
        yield return new WaitForSeconds(1f);
        SpawnObstacle(randomIndex);
    }
}
