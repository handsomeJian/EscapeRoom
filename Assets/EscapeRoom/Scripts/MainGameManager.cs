using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
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

    public UnityEvent OnGameStart;
    public UnityEvent OnLevel1End;
    public UnityEvent OnLevel2End;
    public UnityEvent OnGameEnd;

    public GameObject[] VisualEffects;

    public OVRPassthroughLayer PassthroughLayer;

    public static MainGameManager instance;

    bool hasStarted = false;
    [SerializeField] private TMP_Text countdownText; // Assign in the inspector by dragging the Text component here.
    [SerializeField] private float countdownTime = 720; // Countdown time in seconds.
    [SerializeField] private GameObject timeUpImg;
    void Start()
    {
       
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;


        //L2Scrpit.isWin.
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            hasStarted = true;
            StartL1();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            StartCoroutine(winLevel1());
            level1Won = true; // Prevent coroutine from being called again
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartCoroutine(winLevel2());
            level2Won = true; // Prevent coroutine from being called again
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

    public void WinL3()
    {
        StartCoroutine(winLevel3());
    }

    public void OnReceiveStartMessage(string msg)
    {
        if (hasStarted)
            return;
        hasStarted = true;
        StartL1();
    }

    public void StartL1()
    {
        StartCoroutine(StartLevel1());
    }


    IEnumerator StartLevel1()
    {
        yield return new WaitForSeconds(3f);
        audioSource.clip = audioClips[0];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        OnGameStart.Invoke();
        L1Game.SetActive(true);
        L1Arrow.SetActive(true);
        StartCoroutine(StartCountdown());
    }
    IEnumerator winLevel1()
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        L1Game.SetActive(false);
        L1Arrow.SetActive(false);
        yield return new WaitForSeconds(2f);

        OnLevel1End.Invoke();

        
        L2Game.SetActive(true);
        L2Arrow.SetActive(true);
        GameProcessManager.instance.TriggerSceneMesh(false);
        //play audio 2: player1,3: JJ, 4:player2, 
        audioSource.clip = audioClips[2];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        audioSource.clip = audioClips[3];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        audioSource.clip = audioClips[4];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        

    }
    IEnumerator winLevel2()
    {
        audioSource.clip = audioClips[5];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        //yield return new WaitForSeconds(6f);
        L2Game.SetActive(false);
        L2Arrow.SetActive(false);
        yield return new WaitForSeconds(2f);

        OnLevel2End.Invoke();

        
        L3Game.SetActive(true);

        audioSource.clip = audioClips[6];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        audioSource.clip = audioClips[7];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);
        
    }
    IEnumerator winLevel3()
    {
        audioSource.clip = audioClips[8];
        audioSource.Play();
        yield return new WaitForSeconds(audioSource.clip.length);

        L3Game.SetActive(false);
        
        OnGameEnd.Invoke();
    }

    public void SetL1Passthrough() {
        PassthroughLayer.edgeRenderingEnabled = true;
        PassthroughLayer.SetBrightnessContrastSaturation(-1, -1, -1);
    }

    public void SetL2Passthrough() {
        PassthroughLayer.edgeRenderingEnabled = true;
        PassthroughLayer.edgeColor = Color.white;
        PassthroughLayer.SetBrightnessContrastSaturation(-1, 0.25f, 0);
    }

    public void SetL3Passthrough() {
        PassthroughLayer.edgeRenderingEnabled = false;
        PassthroughLayer.SetBrightnessContrastSaturation(0, 0, 0);
    }

    private IEnumerator StartCountdown()
    {
        float currentTime = countdownTime;
        while (currentTime > 0)
        {
            // Calculate minutes and seconds from currentTime.
            int minutes = Mathf.FloorToInt(currentTime / 60);
            int seconds = Mathf.FloorToInt(currentTime % 60);

            // Update UI Text to show time in "minutes:seconds" format.
            countdownText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Wait for a second before updating the countdown.
            yield return new WaitForSeconds(1f);

            // Decrease current time by one second.
            currentTime--;
        }

        // When the countdown is over, change the scale of the text object.
        countdownText.text = "00:00";
        timeUpImg.SetActive(true);
    }
}
