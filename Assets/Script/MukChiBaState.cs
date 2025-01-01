using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MukChiBaState : IState
{
    private GameManager gameManager;

    public MukChiBaState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("묵찌빠 상태 시작");
    }

    public void Exit()
    {
        gameManager.PlayerChoice = Choice.None;
        gameManager.ComputerChoice = Choice.None;
    }

    public void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            gameManager.PlayerChoice = Choice.Rock;
        else if (Input.GetKeyDown(KeyCode.X))
            gameManager.PlayerChoice = Choice.Scissors;
        else if (Input.GetKeyDown(KeyCode.C))
            gameManager.PlayerChoice = Choice.Paper;
    }

    public void Update()
    {
        if (gameManager.PlayerChoice != Choice.None)
        {
            gameManager.ComputerChoice = gameManager.GetRandomChoice();
            DetermineMukChiBaWinner();
        }
    }

    public void MonsterTurn()
    {
        GameManager.Instance.ComputerChoice = GameManager.Instance.GetRandomChoice();
    }

    private void DetermineMukChiBaWinner()
    {
        if (gameManager.PlayerChoice == gameManager.ComputerChoice)
        {
            if (gameManager.IsPlayerAttacking)
            {
                Debug.Log("플레이어 최종 승리! 게임 종료");
                gameManager.StateMachine.ChangeState(new GameOverState(gameManager));
            }
            else
            {
                Debug.Log("컴퓨터 최종 승리! 게임 종료");
                gameManager.StateMachine.ChangeState(new GameOverState(gameManager));
            }
        }
        else if ((gameManager.PlayerChoice == Choice.Rock && gameManager.ComputerChoice == Choice.Scissors) ||
                 (gameManager.PlayerChoice == Choice.Scissors && gameManager.ComputerChoice == Choice.Paper) ||
                 (gameManager.PlayerChoice == Choice.Paper && gameManager.ComputerChoice == Choice.Rock))
        {
            Debug.Log("플레이어가 공격권을 가져갑니다.");
            gameManager.IsPlayerAttacking = true;
        }
        else
        {
            Debug.Log("컴퓨터가 공격권을 가져갑니다.");
            gameManager.IsPlayerAttacking = false;
        }

        gameManager.PlayerChoice = Choice.None;
        gameManager.ComputerChoice = Choice.None;
    }
}
