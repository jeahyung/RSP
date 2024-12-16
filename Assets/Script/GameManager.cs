using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Choice { None, Rock, Paper, Scissors, TimeOut }

public class GameManager : MonoBehaviour
{
    public GameStateMachine StateMachine { get; private set; }
    public Choice PlayerChoice { get; set; }
    public Choice ComputerChoice { get; set; }
    public bool IsPlayerAttacking { get; set; }
    public bool IsOnePlaying = false;
    
    private Choose choose;
    [SerializeField] private int stageCnt = 10;
    [SerializeField] private int nowCnt = 1;

    //[SerializeField] private int hp = 3;
    public int hp = 4;

    //------------------타이머 관련 ---------------------------
    private float inputTimer = 0f;
    private const float INPUT_TIME_LIMIT = 2f;
    private bool isInputPhase = true;
    //------------------타이머 관련 ---------------------------


    private void Start()
    {
        StateMachine = new GameStateMachine();
        StateMachine.ChangeState(new RockPaperScissorsState(this));

        choose = FindObjectOfType<Choose>();
    }

    private void Update()
    {
        if (isInputPhase)
        {
            inputTimer += Time.deltaTime;
            StateMachine.HandleInput();

            if (inputTimer >= INPUT_TIME_LIMIT)
            {
                isInputPhase = false;
                inputTimer = 0f;
                LockPlayerInput();
            }
        }
        else
        {
            isInputPhase = true;

            if (IsOnePlaying)
            {
                StateMachine.Update();
            }
        }
        //    StateMachine.HandleInput();
        //if (IsOnePlaying) { StateMachine.Update(); }        
    }

    private void LockPlayerInput()
    {
        // 플레이어 입력을 막는 로직

        Debug.Log("입력 시간 종료. 결과를 계산합니다.");
    }

    public void ChooseUi(Choice c)
    {
        choose.ChoiceThat(c);
    }

    public Choice GetRandomChoice()
    {
        return (Choice)UnityEngine.Random.Range(1, 4);
    }

    public void StartWinSequence()
    {
        StartCoroutine(Win());
    }

    private IEnumerator Win()
    {
        yield return new WaitForSeconds(0.1f); //연출을 위한 지연

        choose.ResetAllImages();
        nowCnt++;
        if(nowCnt > stageCnt)
        {
            StateMachine.ChangeState(new MukChiBaState(this));  //보스 스테이지로 근데 이거 어떻게 여러개로 처리할지 고민중
            nowCnt = 0;
        }
    }
    
    public void StartLoseSequence()
    {
        StartCoroutine(Lose());

    }

    private IEnumerator Lose()
    {
        yield return new WaitForSeconds(0.1f); //연출을 위한 지연

        choose.ResetAllImages();
        if(nowCnt <= 1)
        {
            hp--;
            if(hp <= 0)
            {
                GameOver();
            }
        }
        else
        {
            nowCnt--;
        }
    }

    public void GameOver()
    {
        Debug.Log("게임 오버!");
        PlayerChoice = Choice.None;
        ComputerChoice = Choice.None;
    }
}
