using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine //MonoBehaviour ��� ���� 
{
    private IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void MonsterTurn()
    {
        currentState?.MonsterTurn();
    }
}
