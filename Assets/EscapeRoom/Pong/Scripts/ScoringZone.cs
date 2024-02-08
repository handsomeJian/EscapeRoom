using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class ScoringZone : MonoBehaviour
{
    public UnityEvent scoreTrigger;

    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("Scoring Trigger");
        if (other.gameObject.TryGetComponent<Ball>(out var ball))
        {
            //Debug.Log("Scoring");
            scoreTrigger.Invoke();
        }
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ball>(out var ball)) {
            Debug.Log("Scoring");
            scoreTrigger.Invoke();
        }
    }*/

}
