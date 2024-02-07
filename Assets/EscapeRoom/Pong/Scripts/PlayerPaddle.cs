using UnityEngine;

public class PlayerPaddle : Paddle
{
    private Vector3 direction;

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) ) {//|| Input.GetKey(KeyCode.UpArrow)
            direction = transform.parent.TransformDirection(Vector3.up);
        } else if (Input.GetKey(KeyCode.S) ) {//|| Input.GetKey(KeyCode.DownArrow)
            direction = transform.parent.TransformDirection(Vector3.down);
        } else {
            direction = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        if (direction.sqrMagnitude != 0) {
            rb.AddForce(direction * speed);
        }
    }

}
