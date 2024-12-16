using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMachine //MonoBehaviour 상속 제거 
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
}
