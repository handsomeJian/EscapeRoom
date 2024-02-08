using UnityEngine;

public abstract class Paddle : MonoBehaviour
{
    protected Rigidbody rb;

    public float speed = 8f;
    [Tooltip("Changes how the ball bounces off the paddle depending on where it hits the paddle. The further from the center of the paddle, the steeper the bounce angle.")]
    public bool useDynamicBounce = false;
    public float maxBounceAngle = 75f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ResetPosition()
    {
        rb.velocity = Vector3.zero;
        //rb.position = new Vector2(rb.position.x, 0f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (useDynamicBounce && collision.gameObject.CompareTag("Ball"))
        {
            /*
            Rigidbody ball = collision.rigidbody;//Rigidbody2D ball = collision.rigidbody;
            Collider paddle = collision.collider;

            // Gather information about the collision
            Vector3 ballDirection = ball.velocity.normalized;
            Vector3 contactDistance = ball.transform.position - paddle.bounds.center;
            Vector3 surfaceNormal = collision.GetContact(0).normal;
            Vector3 rotationAxis = Vector3.Cross(Vector3.up, surfaceNormal);

            // Rotate the direction of the ball based on the contact distance
            // to make the gameplay more dynamic and interesting
           
            float bounceAngle = (contactDistance.y / paddle.bounds.size.y) * maxBounceAngle;
            ballDirection = Quaternion.AngleAxis(bounceAngle, rotationAxis) * ballDirection;
            //ballDirection = Vector3.Reflect(ballDirection, surfaceNormal);

            // Re-apply the new direction to the ball
            ball.velocity = ballDirection * ball.velocity.magnitude;
            */
            Rigidbody ballRigidbody = collision.rigidbody;
            Collider paddleCollider = collision.collider;

            // Gather information about the collision
            Vector3 ballVelocity = ballRigidbody.velocity;
            Vector3 contactPoint = collision.GetContact(0).point;
            Vector3 paddleCenter = paddleCollider.bounds.center;

            // Calculate the relative position of the ball when it hits the paddle
            float relativePosition = (contactPoint.x - paddleCenter.x) / paddleCollider.bounds.size.x;

            // Calculate the bounce angle based on the relative position of the hit
            float bounceAngle = relativePosition * maxBounceAngle;

            // Calculate the new direction of the ball
            float angleInRadians = bounceAngle * Mathf.Deg2Rad;
            Vector3 newVelocityDirection = new Vector3(Mathf.Sin(angleInRadians), Mathf.Cos(angleInRadians), 0);

            // Adjust the direction based on the side of the paddle hit
            if (ballVelocity.x > 0)
            {
                newVelocityDirection.x = -Mathf.Abs(newVelocityDirection.x);
            }
            else
            {
                newVelocityDirection.x = Mathf.Abs(newVelocityDirection.x);
            }

            // Apply the new velocity to the ball, maintaining its current speed
            ballRigidbody.velocity = newVelocityDirection.normalized * ballVelocity.magnitude;





        }
    }

}
