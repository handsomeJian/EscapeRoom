using UnityEngine;

public class ComputerPaddle : Paddle
{
    [SerializeField]
    private Rigidbody ball;
    public float fixedX = 0f; // The fixed local X position you want to maintain
    private void FixedUpdate()
    {
        // Check if the ball is moving towards the paddle (positive x velocity)
        // or away from the paddle (negative x velocity)

        var ballLocalVel = transform.parent.InverseTransformVector(ball.velocity);
        var ballLocalPos = transform.parent.InverseTransformPoint(ball.position);
        var localPos = transform.localPosition;
        //Vector3 currentPosition = transform.localPosition;
        //transform.localPosition = new Vector3(fixedX, currentPosition.y, currentPosition.z);
        if (ballLocalVel.x > 0f)
        {
            // Move the paddle in the direction of the ball to track it
            if (ballLocalPos.y > localPos.y) {
                rb.AddForce(transform.parent.TransformDirection(Vector3.up) * speed);
            } else if (ballLocalPos.y < localPos.y) {
                rb.AddForce(transform.parent.TransformDirection(Vector3.down) * speed);
            }
        }
        else
        {
            // Move towards the center of the field and idle there until the
            // ball starts coming towards the paddle again
            if (localPos.y > 0f) {
                rb.AddForce(transform.parent.TransformDirection(Vector3.down) * speed);
            } else if (localPos.y < 0f) {
                rb.AddForce(transform.parent.TransformDirection(Vector3.up) * speed);
            }
        }
    }

}
