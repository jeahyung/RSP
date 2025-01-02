using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public enum Choice { None, Rock, Paper, Scissors, TimeOut }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameStateMachine StateMachine { get; private set; }
    public Choice PlayerChoice { get; set; }
    public Choice ComputerChoice { get; set; }
    public bool IsPlayerAttacking { get; set; }
    public bool IsOnePlaying = false;
    
    private Choose choose;
    private bool monster = true;

    [SerializeField] private int stageCnt = 10;
    [SerializeField] private int nowCnt = 1;

    //[SerializeField] private int hp = 3;
    public int hp = 3;
    private bool hpCnt = false;
    //------------------타이머 관련 ---------------------------
    private float inputTimer = 0f;
    private const float INPUT_TIME_LIMIT = 2f;
    private bool isInputPhase = true;
    //------------------타이머 관련 ---------------------------

    public TextMeshProUGUI text;
    public event Action OnWin;
    public event Action winMoveMap;
    public event Action loseMoveMap;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StateMachine = new GameStateMachine();
        StateMachine.ChangeState(new RockPaperScissorsState());

        choose = FindObjectOfType<Choose>();
        
        text.text = nowCnt.ToString();
    }

    private void Update()
    {
        if (isInputPhase)
        {
            inputTimer += Time.deltaTime;
            StateMachine.HandleInput();
            //몬스터 애니메이션 재생을 위한 조건문
            //monster 몬스터 애니메이션의 반복 재생을 막기 위한 bool 변수
            if (inputTimer >= INPUT_TIME_LIMIT - 0.5f)
            {
                if(monster)
                {
                    StateMachine.MonsterTurn();
                    monster = false;
                }
            }
            //입력 시간 종료시 isInputPhase false로 넘기면서 판정으로 넘어감
            if (inputTimer >= INPUT_TIME_LIMIT)
            {
                isInputPhase = false;
                inputTimer = 0f;
                LockPlayerInput();
            }
        }
        else
        {            
            if (IsOnePlaying)
            {
                StateMachine.Update();
                //isInputPhase 타이머 재시작을 위한 불 변수
                isInputPhase = true;        //이걸 화면전환이나 이벤트로 관리시 1층에서 게임의 재시작에 대한 문제가 발생할 우려가 있음
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
        monster = true;

        yield return new WaitForSeconds(0.1f); //연출을 위한 지연
       

        choose.ResetAllImages();
        nowCnt++;
        if(nowCnt > stageCnt)
        {
            StateMachine.ChangeState(new MukChiBaState(this));  //보스 스테이지로 근데 이거 어떻게 여러개로 처리할지 고민중
            nowCnt = 0;
        }
        winMoveMap?.Invoke();
        text.text = nowCnt.ToString();
    }
    
    public void StartLoseSequence()
    {
        StartCoroutine(Lose());

    }

    private IEnumerator Lose()
    {
        monster = true;
        yield return new WaitForSeconds(0.1f); //연출을 위한 지연
        
        choose.ResetAllImages();
        if(nowCnt <= 1) //1층에서 한판 졌을때
        {
            if (hpCnt)      //연속 패배 체크를 위한 hpCnt변수 사용
            {
                hp--;
                OnWin?.Invoke();
                hpCnt = false;
            }            
            if(hp <= 0)
            {
                GameOver();                               
            }
            else  //처음 질 경우 hpCnt가 true로 되며 연속으로 질 경우만 hp감소
            {
                hpCnt = true;
            }
        }
        else            //1층 아닐때 한판 진경우
        {
            nowCnt--;
            loseMoveMap?.Invoke();// 맵 이동
        }
        
        text.text = nowCnt.ToString();
        
       // OnWin?.Invoke();    //OnWin 임시 이름인데 불편하네;; 이거 하트 감소 시키는 이벤트
    }

    public void ReGame()
    {
        choose.ResetAllImages();
    }

    public void GameOver()
    {
        Debug.Log("게임 오버!");
        PlayerChoice = Choice.None;
        ComputerChoice = Choice.None;
    }
}
