using UnityEngine;

public class PlayerPaddle1 : Paddle
{
    private Vector2 direction;

    private void Update()
    {
        if ( Input.GetKey(KeyCode.UpArrow))//Input.GetKey(KeyCode.W) ||
        {
            direction = Vector2.up;
        }
        else if ( Input.GetKey(KeyCode.DownArrow))//Input.GetKey(KeyCode.S) ||
        {
            direction = Vector2.down;
        }
        else
        {
            direction = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (direction.sqrMagnitude != 0)
        {
            rb.AddForce(direction * speed);
        }
    }

}
