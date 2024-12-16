using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverState : IState
{
    private GameManager gameManager;

    public GameOverState(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    public void Enter()
    {
        Debug.Log("게임 종료");
    }

    public void Exit() { }

    public void HandleInput() { }

    public void Update() { }
}