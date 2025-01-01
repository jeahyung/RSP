using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Animator ani;


    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        //선택과 동시에 none로 초기화되는 문제;;;
        switch (GameManager.Instance.ComputerChoice)
        {
            case Choice.Paper:
                ani.SetTrigger("Hurt");
                break;
             
            case Choice.Scissors:
                ani.SetTrigger("Roll");
                break;

            case Choice.Rock:
                Debug.Log("Attack@@@@@@@@@@@@@@");
                ani.SetTrigger("Attack");
                break;
        }
    }

}
