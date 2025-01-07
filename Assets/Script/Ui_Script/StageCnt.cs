using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;

public class StageCnt : MonoBehaviour
{
    public GameObject[] hearts;
    
    public int userHp;


    void Start()
    {
    
        GameManager.Instance.hpDown += CreateHearts;
    }


    public void CreateHearts()
    {
        userHp = GameManager.Instance.hp;

        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < userHp)
            {
                hearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(false);
            }
        }
    }




}
