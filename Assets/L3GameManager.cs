using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class L3GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] positions; // An array of positions to teleport to
    [SerializeField] private int currentPositionIndex = 0; // Keep track of the current position index
    [SerializeField] private GameObject L3JJ;
    [SerializeField] private GameObject teleportVFX;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("space key was pressed");
            //Teleport();
            StartCoroutine(TeleportAfterVFX());
        }
    }
    public void Teleport()
    {
        if (positions.Length == 0) return; // Exit if no positions are set

        // Move to the next position in the array, looping back to the start if necessary
        currentPositionIndex = (currentPositionIndex + 1) % positions.Length;
        L3JJ.transform.position = positions[currentPositionIndex].position;// + new Vector3(0,0.2f,0);
    }

    IEnumerator TeleportAfterVFX()
    {
        // Exit if no positions are set
        if (positions.Length == 0) yield break;
        
        // Play the teleport VFX
        teleportVFX.SetActive(true);
        //teleportVFX.GetComponent<VisualEffect>().Play();


        // Wait for the VFX to finish
        yield return new WaitForSeconds(.8f);
        if (currentPositionIndex == 2)
        {
            L3JJ.SetActive(false);
        }
        else
        { // Move to the next position in the array, looping back to the start if necessary
            currentPositionIndex = (currentPositionIndex + 1) % positions.Length;
            L3JJ.transform.position = positions[currentPositionIndex].position + new Vector3(0, 0.6f, 0);
           //yield return new WaitForSeconds(.2f);
            teleportVFX.SetActive(false);

        }
       
    }

}
