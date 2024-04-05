using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class L2Attack : MonoBehaviour
{
    [SerializeField] private Transform target; // Assign the target location in the inspector
    [SerializeField] private float height = 10.0f, duration = 2.0f; // Speed at which the object moves to the target
    [SerializeField] private Animator JJlevel2Anim;
    private bool isCollided = false;
    private float timer;
    private Vector3 startPosition;


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player");
        if (other.gameObject.CompareTag("Player"))
        {
            isCollided = true; // Trigger movement when colliding with the player
            startPosition = transform.position;
            timer = 0;
        }
    }


    private void Update()
    {
        if (isCollided )//|| Input.GetKeyDown(KeyCode.Space)
        {
            timer += Time.deltaTime;
            float normalizedTime = timer / duration; // Normalizes time to a 0-1 range

            if (normalizedTime > 1f)
            {
                Destroy(gameObject);
            }
            else
            {
                transform.position = ParabolicLerp(startPosition, target.position, height, normalizedTime);
            }
            StartCoroutine(PlayDamageAnmation());
        }
    }

    private Vector3 ParabolicLerp(Vector3 start, Vector3 end, float height, float t)
    {
        float parabolicT = t * 2 - 1;
        if (Mathf.Abs(start.y - end.y) < 0.1f)
        {
            // Start and end are roughly level, simple linear lerp
            return Vector3.Lerp(start, end, t) + Vector3.up * (height * (1 - parabolicT * parabolicT));
        }
        else
        {
            // Start and end are not level, arc is offset vertically
            Vector3 travelDirection = end - start;
            Vector3 levelDirection = end - new Vector3(start.x, end.y, start.z);
            Vector3 right = Vector3.Cross(travelDirection, levelDirection);
            Vector3 up = Vector3.Cross(right, levelDirection);

            if (end.y > start.y) height = -height;

            return Vector3.Lerp(start, end, t) + up.normalized * (height * (1 - parabolicT * parabolicT));
        }
    }
    private IEnumerator PlayDamageAnmation()
    {
        yield return new WaitForSeconds(1f);
        JJlevel2Anim.SetTrigger("takeDamage");
    }
}
