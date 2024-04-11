using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DollRotationScript : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var currentRot = transform.rotation.eulerAngles;
        currentRot.y += rotationSpeed * Time.deltaTime;
        if (currentRot.y > 360)
        {
            currentRot.y -= 360;
        }

        var newRot = transform.rotation;
        newRot.eulerAngles = currentRot;
        transform.rotation = newRot;
    }
}
