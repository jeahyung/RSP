using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 targetPosition;
    public float moveDuration;

    private Vector2 originalPosition;
    public float returnDelay = 1f;
    
    void Start()
    {
        originalPosition = transform.position;
        GameManager.Instance.winMovePlayer += MovePlayer;

        targetPosition = new Vector2(originalPosition.x + 5f, originalPosition.y);
    }

    void Update()
    {
        
    }

    public void MovePlayer()
    {
        transform.DOMove(targetPosition, moveDuration)
                    .SetEase(Ease.OutQuad)
                    .OnComplete(() => {
                        transform.position = originalPosition;
                    });
    }

}
