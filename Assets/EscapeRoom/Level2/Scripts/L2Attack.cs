using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2Attack : MonoBehaviour
{
    [SerializeField] private Transform target; // Assign the target location in the inspector
    [SerializeField] private float speed = 5f; // Speed at which the object moves to the target
    private bool isCollided = false;

    private void Update()
    {
        if (isCollided)
        {
            // Move towards the target location
            transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

            // Check if the object has reached the target location
            if (transform.position == target.position)
            {
                Destroy(gameObject); // Destroy the object
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player");
        if (other.gameObject.CompareTag("Player"))
        {
            isCollided = true; // Trigger movement when colliding with the player
        }
    }
}
