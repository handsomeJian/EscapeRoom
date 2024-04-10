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
        if (dist < distThreadhold || Input.GetKeyDown("space" )&& !hasTeleported[currentPositionIndex])//||Input.GetKeyDown("space"
        {
            
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
        password[currentPositionIndex].SetActive(true);
        StartCoroutine(Bounce(password[currentPositionIndex]));
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


    IEnumerator Bounce(GameObject target)
    {
        Vector3 originalPosition = target.transform.localPosition;
        Vector3 upPosition = originalPosition + new Vector3(0, 0.7f, 0); // Move up by 0.2 units

        // Move up
        float elapsedTime = 0;
        float duration = 0.2f; // Duration of the up movement
        while (elapsedTime < duration)
        {
            target.transform.localPosition = Vector3.Lerp(originalPosition, upPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Move down
        elapsedTime = 0;
        while (elapsedTime < duration)
        {
            target.transform.localPosition = Vector3.Lerp(upPosition, originalPosition, (elapsedTime / duration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the GameObject is exactly back at its original position
        target.transform.localPosition = originalPosition;
    }
}
