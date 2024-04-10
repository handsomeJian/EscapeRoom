using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckCollision : MonoBehaviour
{
    [SerializeField] private L3GameManager L3Script;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
