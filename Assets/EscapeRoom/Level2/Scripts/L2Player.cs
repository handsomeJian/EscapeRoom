using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2Player : MonoBehaviour
{
    [SerializeField] private Transform[] tracks; // Assign in inspector, positions of each track
    [SerializeField] private int currentTrackIndex = 1; // Start in the middle track
    [SerializeField] private float jumpSpeed = 5.0f; // Initial jump speed
    [SerializeField] private float gravity = 9.8f; // Gravity strength
    [SerializeField] private float moveSpeed = 0.2f; // Speed of horizontal movement
    [SerializeField] private float pushBackDuration = 1f;
    [SerializeField] private bool isPushedBack = false; 
    private float verticalVelocity = 0.0f; // Current vertical speed
    private bool isGrounded = true;
    void Update()
    {
        MoveHorizontally();
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
            
        }
        if (!isGrounded)
        {
            Fall(currentTrackIndex);
        }
    }

    void MoveHorizontally()
    {
        if (transform.localPosition.x > 0)
        {
            float moveHorizontal = transform.localPosition.x * moveSpeed;
            transform.localPosition -= new Vector3(moveSpeed * Time.deltaTime, 0, 0);//moveHorizontal
        }

    }

    void MoveDown()
    {
        if (currentTrackIndex < tracks.Length - 1)
        {
            currentTrackIndex++;
            MoveToTrack(currentTrackIndex);
        }
    }

    void MoveUp()
    {
        if (currentTrackIndex > 0)
        {
            currentTrackIndex--;
            MoveToTrack(currentTrackIndex);
        }
    }

    void MoveToTrack(int trackIndex)
    {
        Vector3 localPos = transform.localPosition;//localPosition
        Vector3 newPosition = new Vector3(localPos.x, tracks[trackIndex].localPosition.y+0.5f, localPos.z);
        transform.localPosition = newPosition;
    }

    void Jump()
    {
        isGrounded = false; // Object is now in the air
        verticalVelocity = jumpSpeed; // Set initial upward velocity
    }
    void Fall(int trackIndex)
    {
        Vector3 localPos = transform.localPosition;//localPosition
        //position before jump
        float yBefore = tracks[trackIndex].position.y+0.5f;
        // Apply gravity
        verticalVelocity -= gravity * Time.deltaTime;
        // Move object vertically
        transform.localPosition += new Vector3(0, verticalVelocity * Time.deltaTime, 0);

        // Check if object has landed
        if (transform.position.y <= yBefore) // Assuming ground is at y=0
        {
            // Correct position to ground level and reset states
            transform.localPosition = new Vector3(localPos.x, yBefore, localPos.z);
            isGrounded = true;
            verticalVelocity = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("collision");
            // Move the player backward
            transform.localPosition += new Vector3(moveSpeed * pushBackDuration, 0, 0); // Use pushBackDuration to determine the pushback distance
            other.gameObject.SetActive(false);
            StartCoroutine(ResumeForwardMovement());
        }
    }

    IEnumerator ResumeForwardMovement()
    {
        isPushedBack = true; // Disable normal movement logic
        yield return new WaitForSeconds(pushBackDuration); // Wait for the duration
        isPushedBack = false; // Re-enable normal movement logic
    }

}


