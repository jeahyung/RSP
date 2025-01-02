using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UiEffect : MonoBehaviour
{
    //gameManager.winMoveMap += WinFloor;    
    public Image[] targetImage;
    public float moveDuration = 1f;
    public float returnDelay = 0.5f;

    private Vector3[] originalPosition;
    RectTransform[] rectTransforms;

    private void Start()
    {
        originalPosition = new Vector3[targetImage.Length];
        rectTransforms = new RectTransform[targetImage.Length];

        for (int i = 0; i < targetImage.Length; i++)
        {
            originalPosition[i] = targetImage[i].transform.position;//rectTransform.anchoredPosition3D;
            rectTransforms[i] = targetImage[i].GetComponent<RectTransform>();
        }

        GameManager.Instance.winMoveMap += WinMoveAndReturn;
        GameManager.Instance.loseMoveMap += LoseMoveAndReturn;
    }

    public void WinMoveAndReturn()
    {
        Debug.Log("asdf");
        targetImage[0].rectTransform.DOAnchorPos(rectTransforms[1].anchoredPosition, moveDuration)
            .OnComplete(() => {
                //targetImage[0].rectTransform.DOAnchorPos3D(originalPosition[0], moveDuration)
                //    .SetDelay(returnDelay);
                targetImage[0].transform.position = originalPosition[0];
            });
    }

    public void LoseMoveAndReturn()
    {
        targetImage[1].rectTransform.DOAnchorPos(rectTransforms[0].anchoredPosition, moveDuration)
            .OnComplete(() => {
                //targetImage[1].rectTransform.DOAnchorPos3D(originalPosition[1], moveDuration)
                //    .SetDelay(returnDelay);

                targetImage[1].transform.position = originalPosition[1];
            });
    }
}