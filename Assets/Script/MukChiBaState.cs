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
        Debug.Log("����� ���� ����");
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
                Debug.Log("�÷��̾� ���� �¸�! ���� ����");
                gameManager.StateMachine.ChangeState(new GameOverState(gameManager));
            }
            else
            {
                Debug.Log("��ǻ�� ���� �¸�! ���� ����");
                gameManager.StateMachine.ChangeState(new GameOverState(gameManager));
            }
        }
        else if ((gameManager.PlayerChoice == Choice.Rock && gameManager.ComputerChoice == Choice.Scissors) ||
                 (gameManager.PlayerChoice == Choice.Scissors && gameManager.ComputerChoice == Choice.Paper) ||
                 (gameManager.PlayerChoice == Choice.Paper && gameManager.ComputerChoice == Choice.Rock))
        {
            Debug.Log("�÷��̾ ���ݱ��� �������ϴ�.");
            gameManager.IsPlayerAttacking = true;
        }
        else
        {
            Debug.Log("��ǻ�Ͱ� ���ݱ��� �������ϴ�.");
            gameManager.IsPlayerAttacking = false;
        }

        gameManager.PlayerChoice = Choice.None;
        gameManager.ComputerChoice = Choice.None;
    }
}
