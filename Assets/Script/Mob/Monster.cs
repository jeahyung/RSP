using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private Animator selectedAnimator;
    public List<GameObject> gameObjects = new List<GameObject>();

    void Start()
    {
        ActicateRandomObject();

        GameManager.Instance.monsterTurn += SwitchingMobAni;
        GameManager.Instance.changeMob += ActicateRandomObject;        
    }

    public void ActicateRandomObject()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }
        int randomIndex = Random.Range(0, gameObjects.Count);
        GameObject selectedObject = gameObjects[randomIndex];
        selectedObject.SetActive(true);
        selectedAnimator = selectedObject.GetComponent<Animator>();
    }

    public void SwitchingMobAni()
    {
        if (selectedAnimator == null)
        {
            Debug.LogWarning("No animator selected");
            return;
        }

        switch (GameManager.Instance.ComputerChoice)
        {
            case Choice.Paper:
                selectedAnimator.SetTrigger("Hurt");
                break;

            case Choice.Scissors:
                selectedAnimator.SetTrigger("Roll");
                break;

            case Choice.Rock:
                selectedAnimator.SetTrigger("Attack");
                break;
        }
    }
}
