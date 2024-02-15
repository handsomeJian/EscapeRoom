using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerPaddle : Paddle
{
    private Vector3 direction;
    public float fixedX = 0f;
    [SerializeField] private Transform MaxPos, MinPos;
    public int dist = 0;
    public int minDis = 5, maxDis = 60;

    void Start()
    {
        
    }
    private void Update()
    {

        //Vector3 currentPosition = transform.localPosition;


        var moveVec = MaxPos.position - MinPos.position;
        float w = (dist - minDis) * 1.0f / (maxDis - minDis);
        var newPos = MinPos.position + moveVec * w;
        transform.position = newPos;


        /*
        transform.localPosition = new Vector3(fixedX, currentPosition.y, currentPosition.z);
        if (Input.GetKey(KeyCode.W) ) {//|| Input.GetKey(KeyCode.UpArrow)
            direction = transform.parent.TransformDirection(Vector3.up);
        } else if (Input.GetKey(KeyCode.S) ) {//|| Input.GetKey(KeyCode.DownArrow)
            direction = transform.parent.TransformDirection(Vector3.down);
        } else {
            direction = Vector3.zero;
        }
        */
    }

    private void FixedUpdate()
    {
        /*
        if (direction.sqrMagnitude != 0) {
            rb.AddForce(direction * speed);
        }
        */
    }

}
