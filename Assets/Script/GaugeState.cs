using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeState : IState
{
    public GaugeState()
    {
        //this.gameManager = gameManager;
    }
    // GameManager 싱글톤으로 변경 -> 생성자 필요없음

    public void Enter()
    {
        Debug.Log("가위바위보 상태 시작");
    }

    public void Exit()
    {
        ChoiceReset();
    }

    public void HandleInput()
    {
        GameManager.Instance.IsOnePlaying = true;

        //**********************None처리 해야함***************************
        if (Input.GetKeyDown(KeyCode.Z))
        {
            GameManager.Instance.PlayerChoice = Choice.Rock;
            GameManager.Instance.ChooseUi(Choice.Rock);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            GameManager.Instance.PlayerChoice = Choice.Scissors;
            GameManager.Instance.ChooseUi(Choice.Scissors);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            GameManager.Instance.PlayerChoice = Choice.Paper;
            GameManager.Instance.ChooseUi(Choice.Paper);
        }
    }

    public void Update()
    {
        GameManager.Instance.IsOnePlaying = false;

        if (GameManager.Instance.PlayerChoice != Choice.None)
        {
            //GameManager.Instance.ComputerChoice = GameManager.Instance.GetRandomChoice();
            DetermineWinner();
            //inputProcessed = false;          
        }
        else
        {
            Debug.Log("플레이어 패배!");
            //Debug.Log(GameManager.Instance.PlayerChoice);
            GameManager.Instance.IsPlayerAttacking = false;
            //gameManager.StateMachine.ChangeState(new MukChiBaState(gameManager));
            ChoiceReset();
            GameManager.Instance.StartLoseSequence();
        }
    }

    public void MonsterTurn()
    {
        GameManager.Instance.ComputerChoice = GameManager.Instance.GetRandomChoice();
    }

    private void DetermineWinner()
    {
        if (GameManager.Instance.PlayerChoice == GameManager.Instance.ComputerChoice)
        {
            Debug.Log("무승부! 다시 가위바위보");
            Debug.Log(GameManager.Instance.PlayerChoice);
            GameManager.Instance.choiceText[0].text = GameManager.Instance.PlayerChoice.ToString();
            GameManager.Instance.choiceText[1].text = GameManager.Instance.ComputerChoice.ToString();
            ChoiceReset();
            GameManager.Instance.ReGame();
        }
        else if ((GameManager.Instance.PlayerChoice == Choice.Rock && GameManager.Instance.ComputerChoice == Choice.Scissors) ||
                 (GameManager.Instance.PlayerChoice == Choice.Scissors && GameManager.Instance.ComputerChoice == Choice.Paper) ||
                 (GameManager.Instance.PlayerChoice == Choice.Paper && GameManager.Instance.ComputerChoice == Choice.Rock))
        {
            Debug.Log("플레이어 승!");
            Debug.Log(GameManager.Instance.PlayerChoice);
            GameManager.Instance.IsPlayerAttacking = true;
            //묵찌빠 구현 가위바위보 - > 묵찌빠로 넘어감
            //gameManager.StateMachine.ChangeState(new MukChiBaState(gameManager)); 
            GameManager.Instance.choiceText[0].text = GameManager.Instance.PlayerChoice.ToString();
            GameManager.Instance.choiceText[1].text = GameManager.Instance.ComputerChoice.ToString();
            ChoiceReset();
            GameManager.Instance.StartWinSequence();
        }
        else
        {
            Debug.Log("플레이어 패배!");
            Debug.Log(GameManager.Instance.PlayerChoice);
            GameManager.Instance.IsPlayerAttacking = false;
            GameManager.Instance.choiceText[0].text = GameManager.Instance.PlayerChoice.ToString();
            GameManager.Instance.choiceText[1].text = GameManager.Instance.ComputerChoice.ToString();
            //gameManager.StateMachine.ChangeState(new MukChiBaState(gameManager));
            ChoiceReset();
            GameManager.Instance.StartLoseSequence();
        }

    }

    private void ChoiceReset()
    {
        GameManager.Instance.PlayerChoice = Choice.None;
        GameManager.Instance.ComputerChoice = Choice.None;
    }

}
