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

    void Start()
    {
        //L2Scrpit.isWin.
    }

    // Update is called once per frame
    void Update()
    {

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

    IEnumerator winLevel1()
    {
        yield return new WaitForSeconds(3f);
        L1Game.SetActive(false);
        L1Arrow.SetActive(true);
        L2Game.SetActive(true);
    }
    IEnumerator winLevel2()
    {
        yield return new WaitForSeconds(6f);
        L2Game.SetActive(false);
        L2Arrow.SetActive(true);
        L3Game.SetActive(true);
    }
}
