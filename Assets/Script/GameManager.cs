using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public enum Choice { None, Rock, Paper, Scissors, TimeOut }

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI[] choiceText;

    public static GameManager Instance { get; private set; }

    public GameStateMachine StateMachine { get; private set; }
    public Choice PlayerChoice { get; set; }
    public Choice ComputerChoice { get; set; }
    public bool IsPlayerAttacking { get; set; }
    public bool IsOnePlaying = false;
    private bool isGameRunning = true;

    private Choose choose;
    private bool monster = true;

    [SerializeField] private int stageCnt = 10;
    [SerializeField] private int nowCnt = 1;

    //[SerializeField] private int hp = 3;
    public int hp = 3;
    private bool hpCnt = false;
    //------------------Ÿ�̸� ���� ---------------------------
    private float inputTimer = 0f;
    private const float INPUT_TIME_LIMIT = 2f;
    private bool isInputPhase = true;
    //------------------Ÿ�̸� ���� ---------------------------

    private Coroutine gameLoopCoroutine; // ������: ���ο� ���� �߰�


    public TextMeshProUGUI text;
    public event Action hpDown;
    public event Action winMoveMap;
    public event Action loseMoveMap;
    public event Action winMovePlayer;
    public event Action monsterTurn;
    public event Action changeMob;
    

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

        StartGameLoop(); // ������: ���� ���� ����
    }

    private void StartGameLoop()
    {
        isGameRunning = true;

        if (gameLoopCoroutine != null)
        {
            StopCoroutine(gameLoopCoroutine);
        }
        gameLoopCoroutine = StartCoroutine(GameLoopCoroutine());
    }

    private IEnumerator GameLoopCoroutine()
    {
        while (isGameRunning)
        {
            yield return StartCoroutine(InputPhaseCoroutine());
            yield return StartCoroutine(ResultPhaseCoroutine());
        }
    }

    public void StopGame()
    {
        isGameRunning = false;
    }

    private IEnumerator InputPhaseCoroutine()
    {
        isInputPhase = true;
        float inputTimer = 0f;
        monster = true;             //���� ������ �ʿ��� �� �� ����

        while (inputTimer < INPUT_TIME_LIMIT)
        {
            inputTimer += Time.deltaTime;
            StateMachine.HandleInput();

            if ((inputTimer >= 1.0f) && monster)
            {
                StateMachine.MonsterTurn();
                monster = false;
                monsterTurn?.Invoke();
            }

            yield return null;
        }

        isInputPhase = false;
    }

    // ������: ���ο� �ڷ�ƾ �߰�
    private IEnumerator ResultPhaseCoroutine()
    {
        if (IsOnePlaying)
        {
            StateMachine.Update();

            //if (PlayerChoice != Choice.None)                //******************���� ���ΰ� ����****************************************
            //{                                                               //������ ��ü�� ���ϰ� ����
            //    if (IsPlayerAttacking)
            //    {
            //        yield return StartCoroutine(Win());
            //    }
            //    else
            //    {
            //        yield return StartCoroutine(Lose());
            //    }
            //}
            //else
            //{
            //    PlayerChoice = Choice.TimeOut;
            //    yield return StartCoroutine(Lose());
            //}
        }
        //StopGame();
        yield return new WaitForSeconds(0.1f);
    }


    private void Update()
    {
        //if (isInputPhase)
        //{
        //    inputTimer += Time.deltaTime;
        //    StateMachine.HandleInput();
        //    //���� �ִϸ��̼� ����� ���� ���ǹ�
        //    //monster ���� �ִϸ��̼��� �ݺ� ����� ���� ���� bool ����
        //    if ((inputTimer >= INPUT_TIME_LIMIT - 1.0f) && (monster))
        //    {                
        //        StateMachine.MonsterTurn();
        //        monster = false;
        //        monsterTurn?.Invoke();                
        //    }
        //    //�Է� �ð� ����� isInputPhase false�� �ѱ�鼭 �������� �Ѿ
        //    if (inputTimer >= INPUT_TIME_LIMIT)
        //    {
        //        isInputPhase = false;
        //        inputTimer = 0f;                
        //    }
        //}
        //else
        //{            
        //    if (IsOnePlaying)
        //    {
        //        StateMachine.Update();

        //        //isInputPhase Ÿ�̸� ������� ���� �� ����
        //        //isInputPhase = true;        //�̰� ȭ����ȯ�̳� �̺�Ʈ�� ������ 1������ ������ ����ۿ� ���� ������ �߻��� ����� ����
              
        //    }
        //}
        ////    StateMachine.HandleInput();
        ////if (IsOnePlaying) { StateMachine.Update(); }        
    }

    private void LockPlayerInput()
    {
        StateMachine.Update();
        // �÷��̾� �Է��� ���� ����
        //StartLoseSequence();
        Debug.Log("�Է� �ð� ����. ����� ����մϴ�.");
    }

    public void ChooseUi(Choice c)
    {
        choose.ChoiceThat(c);
    }

    public Choice GetRandomChoice()
    {
       // monsterTurn?.Invoke();

        return (Choice)UnityEngine.Random.Range(1, 4);
    }

    public void StartWinSequence()
    {
        //monster = true;

        StartCoroutine(Win());

    }

    private IEnumerator Win()
    {
        monster = true;

        winMovePlayer?.Invoke();    

        choose.ResetAllImages();
        nowCnt++;
        //if(nowCnt > stageCnt)
        //{
        //    StateMachine.ChangeState(new MukChiBaState(this));  //���� ���������� �ٵ� �̰� ��� �������� ó������ �����
        //    nowCnt = 0;                                           //���� �ܰ躰 ü�� �ɱ� ���÷� ����� ���� ������ �������� ����� ���¿��� clear�� ������ 
        //}                                                         //Ŭ���� ���� �Ǵ��� Ŭ����� ������� �ٷ� Ŭ���� ���·� ��ȯ��Ű�� ����
        winMoveMap?.Invoke();
        text.text = nowCnt.ToString();

        changeMob?.Invoke();

        yield return new WaitForSeconds(0.1f); //������ ���� ����
                                               //OnisInputPhase();
       // StartGameLoop(); // ������: ���� ���� �����

    }

    public void OnisInputPhase()
    {
        isInputPhase = true;
    }
   
    public void StartLoseSequence()
    {
//        monster = true;

        StartCoroutine(Lose());
       // Debug.Log("�й�");
    }

    private IEnumerator Lose()
    {
        monster = true;

        choose.ResetAllImages();

        if(nowCnt <= 1) //1������ ���� ������
        {
            OnisInputPhase();

            if (hpCnt)      //���� �й� üũ�� ���� hpCnt���� ���
            {
                hp--;
                //hpCnt = false;
                hpDown?.Invoke();                
            }            
            if(hp <= 0)
            {
                GameOver();                               
            }
            else  //ó�� �� ��� hpCnt�� true�� �Ǹ� �������� �� ��츸 hp����
            {
                hpCnt = !hpCnt;                
            }
        }
        else            //1�� �ƴҶ� ���� �����
        {
            nowCnt--;
            changeMob?.Invoke(); //���� ����

            loseMoveMap?.Invoke();// �� �̵�
            hpCnt = false;
        }
        
        text.text = nowCnt.ToString();

        yield return new WaitForSeconds(0.1f); //������ ���� ����
       // OnisInputPhase();
        //StartGameLoop(); // ������: ���� ���� �����

        // OnWin?.Invoke();    //OnWin �ӽ� �̸��ε� �����ϳ�;; �̰� ��Ʈ ���� ��Ű�� �̺�Ʈ
    }

    public void ReGame()
    {
        monster = true;
       // OnisInputPhase();    
      //  gameStarter = false;
        choose.ResetAllImages();
        //StartGameLoop(); // ������: ���� ���� �����

    }

    public void GameOver()
    {
        Debug.Log("���� ����!");
        PlayerChoice = Choice.None;
        ComputerChoice = Choice.None;        
        //gameStarter = true; //�̷��� ���� ����
    }
}
