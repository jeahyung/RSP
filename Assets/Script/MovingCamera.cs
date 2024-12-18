using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCamera : MonoBehaviour
{
    private float originHeight;

    public float targetHeight;  
    public float duration = 1.5f;

    private GameManager gameManager;
    
    void Start()
    {
        targetHeight = this.transform.position.y;
        originHeight = this.transform.position.y;

        gameManager = FindObjectOfType<GameManager>();
        gameManager.winMoveMap += WinFloor;
        gameManager.loseMoveMap += LoseFloor;
    }

   
    void Update()
    {
    
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    targetHeight = this.transform.position.y + 9f;
        //    transform.DOLocalMoveY(targetHeight, duration).SetEase(Ease.OutQuad).OnComplete(() => {
        //        transform.localPosition = new Vector3(transform.localPosition.x, originHeight, transform.localPosition.z);
        //    }); ;
        //}
    }

    public void WinFloor()
    {
        targetHeight = this.transform.position.y - 9f;
        transform.DOLocalMoveY(targetHeight, duration).SetEase(Ease.OutQuad).OnComplete(() => {
            transform.localPosition = new Vector3(transform.localPosition.x, originHeight, transform.localPosition.z);
        }); ;
    }

    public void LoseFloor()
    {
        targetHeight = this.transform.position.y + 9f;
        transform.DOLocalMoveY(targetHeight, duration).SetEase(Ease.OutQuad).OnComplete(() => {
            transform.localPosition = new Vector3(transform.localPosition.x, originHeight, transform.localPosition.z);
        }); ;
    }
}
