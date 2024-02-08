using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    public float baseSpeed = 5f;
    public float maxSpeed = Mathf.Infinity;
    public float currentSpeed { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void ResetPosition()
    {
        rb.velocity = Vector3.zero;
        rb.position = transform.parent.TransformPoint(new Vector3(8.0f, 0.0f, 0.0f));
        transform.position = transform.parent.TransformPoint(new Vector3(8.0f, 0.0f, 0.0f));
        //Debug.Log("Reset Pos:" + transform.parent.TransformPoint(new Vector3(8.0f, 0.0f, 0.0f)));
    }

    public void AddStartingForce()
    {
        // Flip a coin to determine if the ball starts left or right
        float x = Random.value < 0.5f ? -1f : 1f;

        // Flip a coin to determine if the ball goes up or down. Set the range
        // between 0.5 -> 1.0 to ensure it does not move completely horizontal.
        float y = Random.value < 0.5f ? Random.Range(-1f, -0.5f)
                                      : Random.Range(0.5f, 1f);

        // Apply the initial force and set the current speed
        Vector3 direction = transform.parent.TransformDirection(new Vector3(x, y, 0.0f)).normalized;
        rb.AddForce(direction * baseSpeed, ForceMode.VelocityChange);
        currentSpeed = baseSpeed;

        //Debug.Log("[Ball Start direction]:" + direction.ToString());
    }

    private void FixedUpdate()
    {
        // Clamp the velocity of the ball to the max speed
        Vector3 direction = rb.velocity.normalized;
        currentSpeed = Mathf.Min(currentSpeed, maxSpeed);
        rb.velocity = direction * currentSpeed;
    }

}
