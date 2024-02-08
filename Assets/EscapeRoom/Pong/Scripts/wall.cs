using UnityEngine;

public class wall : MonoBehaviour
{
    public float maxBounceAngle = 75f;

    private void Start()
    {
        Debug.Log("awake");
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ccccc");
        if (collision.gameObject.CompareTag("Ball"))
        {
            Debug.Log("wall");
            Rigidbody ballRigidbody = collision.rigidbody;
            Vector3 incomingVelocity = ballRigidbody.velocity;
            Vector3 normal = collision.contacts[0].normal; // Get the normal of the collision point

            // Reflect the ball's velocity vector off the wall's surface
            Vector3 reflectedVelocity = Vector3.Reflect(incomingVelocity, normal);

            // Since the ball should only move in the XY direction, ensure Z component is zero
            reflectedVelocity.z = 0;

            // Apply the reflected velocity to the ball
            ballRigidbody.velocity = reflectedVelocity;

        }
    }
}
