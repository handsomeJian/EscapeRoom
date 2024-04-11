using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    Vector3 scale;

    [SerializeField] private L3GameManager L3Script;
    // Start is called before the first frame update
    void Start()
    {
        scale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        var offset = 0.1f * Mathf.Sin(Time.time);
        transform.localScale = scale +  Vector3.one * offset;
    }
    void OnTriggerEnter(Collider other) 
    {
        Debug.Log("colldie");
        if (other.gameObject.tag == "L3JJ") 
        {
            
            L3Script.isTrigger = true;
        }
    }
}
