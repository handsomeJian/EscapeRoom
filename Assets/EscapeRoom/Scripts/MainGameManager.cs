using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PongGameManager L1Script;
    [SerializeField] private L2Player L2Scrpit;
    [SerializeField] private GameObject L1Game,L1Arrow, L2Game, L2Arrow, L3Game;
    private bool level1Won = false; 
    private bool level2Won = false;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> audioClips;



    public GameObject[] VisualEffects;

    void Start()
    {
        //L2Scrpit.isWin.
        StartCoroutine(StartLevel1());
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            StartCoroutine(winLevel1());
            level1Won = true; // Prevent coroutine from being called again
        }

        if (L1Script.playerScore == 3 && !level1Won)
        {
            StartCoroutine(winLevel1());
            level1Won = true; // Prevent coroutine from being called again
        }
        if (L2Scrpit.isWin && !level2Won)
        {
            StartCoroutine(winLevel2());
            level2Won = true; // Prevent coroutine from being called again
        }
    }
    IEnumerator StartLevel1()
    {
        yield return new WaitForSeconds(3f);
        audioSource.clip = audioClips[0];
        audioSource.Play();
        //yield return new WaitForSeconds(audioSource.clip.length);
        L1Game.SetActive(true);

    }
    IEnumerator winLevel1()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        L1Game.SetActive(false);
        yield return new WaitForSeconds(2f);
        L1Arrow.SetActive(true);
        L2Game.SetActive(true);
        //play audio
        audioSource.clip = audioClips[2];
        audioSource.Play();
        GameProcessManager.instance.TriggerSceneMesh(false);

    }
    IEnumerator winLevel2()
    {
        audioSource.clip = audioClips[3];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        //yield return new WaitForSeconds(6f);
        L2Game.SetActive(false);
        yield return new WaitForSeconds(2f);
        L1Arrow.SetActive(false);
        L2Arrow.SetActive(true);
        //
        audioSource.clip = audioClips[4];
        audioSource.Play();
        L3Game.SetActive(true);
    }
}
