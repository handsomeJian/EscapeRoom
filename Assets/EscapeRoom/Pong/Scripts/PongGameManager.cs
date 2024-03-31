using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UI;

public class PongGameManager : MonoBehaviour
{
    [SerializeField] private Ball ball;
    [SerializeField] private Paddle playerPaddle;
    [SerializeField] private Paddle computerPaddle;
    [SerializeField] private Text playerScoreText;
    [SerializeField] private Text computerScoreText;
    [SerializeField] private GameObject[] PlayerHealth, ComputerHealth;
    [SerializeField] private GameObject FinalScore;

    [SerializeField] public int playerScore = 0;
    [SerializeField] private int computerScore = 0;
    [SerializeField] private AudioClip[] winSFX;
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        NewGame();
        FinalScore.SetActive(false);

    }

    private void Update()
    {

    }

    public void NewGame()
    {
        SetPlayerScore(0);
        SetComputerScore(0);
        //reset health
        for (int i = 0; i < PlayerHealth.Length; i++)
        {
            PlayerHealth[i].SetActive(true);
            ComputerHealth[i].SetActive(true);
        }

        NewRound();
    }

    public void NewRound()
    {
        //Debug.Log("New Round");
        playerPaddle.ResetPosition();
        computerPaddle.ResetPosition();
        ball.ResetPosition();

        CancelInvoke();
        Invoke(nameof(StartRound), 3f);
    }

    private void StartRound()
    {
        ball.AddStartingForce();
    }

    public void OnPlayerScored()
    {
        playerScore +=1;
        SetPlayerScore(playerScore);
        if (playerScore == 3)
        {
            FinalScore.SetActive(true);
            playerPaddle.ResetPosition();
            computerPaddle.ResetPosition();
            ball.ResetPosition();
        }
        else
        {
            NewRound();
            ComputerHealth[playerScore - 1].SetActive(false); 
            audioSource.clip = winSFX[playerScore - 1];
            audioSource.Play();
        }
        
    }

    public void OnComputerScored()
    {
        computerScore += 1;
        SetComputerScore(computerScore);
        if (computerScore == 3 )
        {
            //restart game if player loose 3 points
            NewGame();
        }
        else
        {
            NewRound();
            PlayerHealth[computerScore-1].SetActive(false);
        }
    }

    private void SetPlayerScore(int score)
    {
        playerScore = score;
        playerScoreText.text = score.ToString();
    }

    private void SetComputerScore(int score)
    {
        computerScore = score;
        computerScoreText.text = score.ToString();
    }

}
