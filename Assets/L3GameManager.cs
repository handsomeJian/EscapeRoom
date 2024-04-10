using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class L3GameManager : MonoBehaviour
{
    [SerializeField] private Transform[] positions; // An array of positions to teleport to
    [SerializeField] private int currentPositionIndex = 0; // Keep track of the current position index
    [SerializeField] private GameObject L3JJ, RightHandAnchor;
    [SerializeField] private GameObject teleportVFX;
    private float dist;
    [SerializeField] private float distThreadhold = 0.5f;
    private bool[] hasTeleported;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;
    public AudioClip LoseSound;
    [SerializeField] private GameObject[] password;
    // Start is called before the first frame update
    void Start()
    {
        hasTeleported = new bool[positions.Length];
            for (int i = 0; i < hasTeleported.Length; i++)
            {
                hasTeleported[i] = false;
            }

    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(L3JJ.transform.position, RightHandAnchor.transform.position);
        if (dist < distThreadhold && !hasTeleported[currentPositionIndex])//||Input.GetKeyDown("space"
        {
            //Debug.Log("space key was pressed");
            //Teleport();
            StartCoroutine(TeleportAfterVFX());
            hasTeleported[currentPositionIndex] = true;
            // Initialize the hasTeleported array with the same length as positions and set all to false
            
        }
    }

    IEnumerator TeleportAfterVFX()
    {
        yield return new WaitForSeconds(1f);
        audioSource.clip = audioClips[currentPositionIndex];
        audioSource.Play();
        

        // Play the teleport VFX
        teleportVFX.SetActive(true);
        //teleportVFX.GetComponent<VisualEffect>().Play();


        // Wait for the VFX to finish
        yield return new WaitForSeconds(audioSource.clip.length);
        //yield return new WaitForSeconds(.8f);
        if (currentPositionIndex == 2)
        {
            L3JJ.SetActive(false);
        }
        else
        { // Move to the next position in the array, looping back to the start if necessary
            currentPositionIndex = (currentPositionIndex + 1) % positions.Length;
            L3JJ.transform.position = positions[currentPositionIndex].position;// new Vector3(0, 0.6f, 0)
           //yield return new WaitForSeconds(.2f);
            teleportVFX.SetActive(false);

        }
        password[currentPositionIndex].SetActive(true);

    }

    public void OnReceiveMessage(string msg)
    {
        if (msg == "Success")
        {
            MainGameManager.instance.WinL3();
        }
        else
        {
            audioSource.clip = LoseSound;
            audioSource.Play();
        }
    }

}
