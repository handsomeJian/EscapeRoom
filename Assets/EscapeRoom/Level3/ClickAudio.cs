using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickAudio : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource audioSource; // Assign the AudioSource in the Unity Inspector
    [SerializeField] private RectTransform targetArea; // The area where the click should not trigger the audio
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        // Check for mouse click
        if (Input.GetMouseButtonDown(0) )//&& !IsPointerOverUIObject()
        {
            // Check if the click is outside the target area
            if (!RectTransformUtility.RectangleContainsScreenPoint(targetArea, Input.mousePosition))
            {
                // Play the audio
                audioSource.Play();
            }
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}
