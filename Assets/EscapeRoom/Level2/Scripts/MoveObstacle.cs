using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float speed = 5.0f;

    void Update()
    {
        transform.localPosition += transform.right * speed * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("End"))
        {

            Destroy(gameObject); // Destroy the obstacle
        }
    }
}
