using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
[RequireComponent(typeof(Rigidbody))]
public class Ball : MonoBehaviour
{
    private Rigidbody rb;

    public float baseSpeed = 5f;
    public float maxSpeed = Mathf.Infinity;
    public float currentSpeed { get; set; }
    public float minHorizontalSpeed = 5f;
    [SerializeField] AudioSource bonce;
    [SerializeField] GameObject ballvfx;
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


    private void OnCollisionEnter(Collision collision)
    {

        Vector3 normal = collision.GetContact(0).normal;
        Vector3 newDirection = Vector3.Reflect(rb.velocity.normalized, normal);

        newDirection = transform.parent.InverseTransformDirection(newDirection);

        // Ensure the ball only moves in the XY direction
        newDirection.z = 0;
        if (Mathf.Abs(newDirection.x) < minHorizontalSpeed)
        {
            newDirection.x = newDirection.x < 0 ? -minHorizontalSpeed : minHorizontalSpeed;
        }

        if (Mathf.Abs(newDirection.y) < minHorizontalSpeed)
        {
            newDirection.y = newDirection.y < 0 ? -minHorizontalSpeed : minHorizontalSpeed;
        }
        // Normalize the new direction to ensure consistent speed
        newDirection = newDirection.normalized;

        newDirection = transform.parent.TransformDirection(newDirection);

        rb.velocity = newDirection * currentSpeed;

        bonce.Play();
        if (collision.gameObject.CompareTag("Paddle"))
        {
            //Debug.Log("paddle");
            //StartCoroutine(ExampleCoroutine());

        }


     }
    IEnumerator ExampleCoroutine()
    {
        //ballvfx.SetActive(true);
        yield return new WaitForSeconds(1);
        //ballvfx.SetActive(false);

    }
}
