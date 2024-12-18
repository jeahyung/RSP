using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPaperScissorsState : IState
{
    private GameManager gameManager;
    //private bool inputProcessed = false;

    public RockPaperScissorsState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("가위바위보 상태 시작");
    }

    public void Exit()
    {
        gameManager.PlayerChoice = Choice.None;
        gameManager.ComputerChoice = Choice.None;
    }

    public void HandleInput()
    {
        gameManager.IsOnePlaying = true;

        //**********************None처리 해야함***************************
        if (Input.GetKeyDown(KeyCode.Z))
        {
            gameManager.PlayerChoice = Choice.Rock;
            gameManager.ChooseUi(Choice.Rock);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            gameManager.PlayerChoice = Choice.Scissors;
            gameManager.ChooseUi(Choice.Scissors);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            gameManager.PlayerChoice = Choice.Paper;
            gameManager.ChooseUi(Choice.Paper);
        }
    }

    public void Update()
    {
        gameManager.IsOnePlaying = false;

        if (gameManager.PlayerChoice != Choice.None)
        {
            gameManager.ComputerChoice = gameManager.GetRandomChoice();
            DetermineWinner();
            //inputProcessed = false;          
        }
    }

    private void DetermineWinner()
    {
        if (gameManager.PlayerChoice == gameManager.ComputerChoice)
        {
            Debug.Log("무승부! 다시 가위바위보");
            Debug.Log(gameManager.PlayerChoice);
            gameManager.PlayerChoice = Choice.None;
            gameManager.ComputerChoice = Choice.None;
            gameManager.ReGame();
        }
        else if ((gameManager.PlayerChoice == Choice.Rock && gameManager.ComputerChoice == Choice.Scissors) ||
                 (gameManager.PlayerChoice == Choice.Scissors && gameManager.ComputerChoice == Choice.Paper) ||
                 (gameManager.PlayerChoice == Choice.Paper && gameManager.ComputerChoice == Choice.Rock))
        {
            Debug.Log("플레이어 승!");
            Debug.Log(gameManager.PlayerChoice);
            gameManager.IsPlayerAttacking = true;
            //묵찌빠 구현 가위바위보 - > 묵찌빠로 넘어감
            //gameManager.StateMachine.ChangeState(new MukChiBaState(gameManager)); 
            gameManager.PlayerChoice = Choice.None;
            gameManager.ComputerChoice = Choice.None;
            gameManager.StartWinSequence();
        }
        else
        {
            Debug.Log("플레이어 패배!");
            Debug.Log(gameManager.PlayerChoice);
            gameManager.IsPlayerAttacking = false;
            //gameManager.StateMachine.ChangeState(new MukChiBaState(gameManager));
            gameManager.PlayerChoice = Choice.None;
            gameManager.ComputerChoice = Choice.None;
            gameManager.StartLoseSequence();
        }
       
    }
}
