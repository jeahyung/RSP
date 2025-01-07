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
    }

    private void Update()
    {
        if (isInputPhase)
        {
            inputTimer += Time.deltaTime;
            StateMachine.HandleInput();
            //���� �ִϸ��̼� ����� ���� ���ǹ�
            //monster ���� �ִϸ��̼��� �ݺ� ����� ���� ���� bool ����
            if (inputTimer >= INPUT_TIME_LIMIT - 1.0f)
            {
                if(monster)
                {
                    StateMachine.MonsterTurn();
                    monster = false;
                    monsterTurn?.Invoke();
                }
            }
            //�Է� �ð� ����� isInputPhase false�� �ѱ�鼭 �������� �Ѿ
            if (inputTimer >= INPUT_TIME_LIMIT)
            {
                isInputPhase = false;
                inputTimer = 0f;                
            }
        }
        else
        {            
            if (IsOnePlaying)
            {
                StateMachine.Update();

                //isInputPhase Ÿ�̸� ������� ���� �� ����
                //isInputPhase = true;        //�̰� ȭ����ȯ�̳� �̺�Ʈ�� ������ 1������ ������ ����ۿ� ���� ������ �߻��� ����� ����
              
            }
        }
        //    StateMachine.HandleInput();
        //if (IsOnePlaying) { StateMachine.Update(); }        
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
        monster = true;

        StartCoroutine(Win());

    }

    private IEnumerator Win()
    {
        winMovePlayer?.Invoke();    

        choose.ResetAllImages();
        nowCnt++;
        //if(nowCnt > stageCnt)
        //{
        //    StateMachine.ChangeState(new MukChiBaState(this));  //���� ���������� �ٵ� �̰� ��� �������� ó������ �����
        //    nowCnt = 0;
        //}
        winMoveMap?.Invoke();
        text.text = nowCnt.ToString();

        changeMob?.Invoke();

        yield return new WaitForSeconds(0.1f); //������ ���� ����
        //OnisInputPhase();
    }
    
    public void OnisInputPhase()
    {
        isInputPhase = true;
    }
    public void StartLoseSequence()
    {
        monster = true;

        StartCoroutine(Lose());
       // Debug.Log("�й�");
    }

    private IEnumerator Lose()
    {

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
        OnisInputPhase();
        // OnWin?.Invoke();    //OnWin �ӽ� �̸��ε� �����ϳ�;; �̰� ��Ʈ ���� ��Ű�� �̺�Ʈ
    }

    public void ReGame()
    {
        monster = true;
        OnisInputPhase();    
      //  gameStarter = false;
        choose.ResetAllImages();
    }

    public void GameOver()
    {
        Debug.Log("���� ����!");
        PlayerChoice = Choice.None;
        ComputerChoice = Choice.None;        
        //gameStarter = true; //�̷��� ���� ����
    }
}
