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
    //------------------Ÿ�̸� ���� ---------------------------
    private float inputTimer = 0f;
    private const float INPUT_TIME_LIMIT = 2f;
    private bool isInputPhase = true;
    //------------------Ÿ�̸� ���� ---------------------------

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
            //���� �ִϸ��̼� ����� ���� ���ǹ�
            //monster ���� �ִϸ��̼��� �ݺ� ����� ���� ���� bool ����
            if (inputTimer >= INPUT_TIME_LIMIT - 0.5f)
            {
                if(monster)
                {
                    StateMachine.MonsterTurn();
                    monster = false;
                }
            }
            //�Է� �ð� ����� isInputPhase false�� �ѱ�鼭 �������� �Ѿ
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
                //isInputPhase Ÿ�̸� ������� ���� �� ����
                isInputPhase = true;        //�̰� ȭ����ȯ�̳� �̺�Ʈ�� ������ 1������ ������ ����ۿ� ���� ������ �߻��� ����� ����
            }
        }
        //    StateMachine.HandleInput();
        //if (IsOnePlaying) { StateMachine.Update(); }        
    }

    private void LockPlayerInput()
    {
        // �÷��̾� �Է��� ���� ����

        Debug.Log("�Է� �ð� ����. ����� ����մϴ�.");
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

        yield return new WaitForSeconds(0.1f); //������ ���� ����
       

        choose.ResetAllImages();
        nowCnt++;
        if(nowCnt > stageCnt)
        {
            StateMachine.ChangeState(new MukChiBaState(this));  //���� ���������� �ٵ� �̰� ��� �������� ó������ �����
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
        yield return new WaitForSeconds(0.1f); //������ ���� ����
        
        choose.ResetAllImages();
        if(nowCnt <= 1) //1������ ���� ������
        {
            if (hpCnt)      //���� �й� üũ�� ���� hpCnt���� ���
            {
                hp--;
                OnWin?.Invoke();
                hpCnt = false;
            }            
            if(hp <= 0)
            {
                GameOver();                               
            }
            else  //ó�� �� ��� hpCnt�� true�� �Ǹ� �������� �� ��츸 hp����
            {
                hpCnt = true;
            }
        }
        else            //1�� �ƴҶ� ���� �����
        {
            nowCnt--;
            loseMoveMap?.Invoke();// �� �̵�
        }
        
        text.text = nowCnt.ToString();
        
       // OnWin?.Invoke();    //OnWin �ӽ� �̸��ε� �����ϳ�;; �̰� ��Ʈ ���� ��Ű�� �̺�Ʈ
    }

    public void ReGame()
    {
        choose.ResetAllImages();
    }

    public void GameOver()
    {
        Debug.Log("���� ����!");
        PlayerChoice = Choice.None;
        ComputerChoice = Choice.None;
    }
}
