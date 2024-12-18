using System.Collections;
using System.Collections.Generic;
//using TMPro;
using UnityEngine;

public class StageCnt : MonoBehaviour
{
    public GameObject[] hearts;
    private GameManager gm;
    public int userHp;


    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        gm.OnWin += CreateHearts;
    }

    private void OnDisable()
    {
        if (gm != null)
        {
            gm.OnWin -= CreateHearts;
        }
    }

    public void CreateHearts()
    {
        userHp = gm.hp;

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
