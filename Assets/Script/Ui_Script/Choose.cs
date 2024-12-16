using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choose : MonoBehaviour
{
    public RectTransform imageZ;
    public RectTransform imageX;
    public RectTransform imageC;

    private Vector3 originalScale;
    private float scaleDuration = 0.5f;
    private float scaleMultiplier = 1.5f;

   // private GameManager gameManager;

    private void Start()
    {
        originalScale = imageZ.localScale;
     //   gameManager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    ScaleImage(imageZ);
        //    ResetOtherImages(imageZ);
        //}
        //else if (Input.GetKeyDown(KeyCode.X))
        //{
        //    ScaleImage(imageX);
        //    ResetOtherImages(imageX);
        //}
        //else if (Input.GetKeyDown(KeyCode.C))
        //{
        //    ScaleImage(imageC);
        //    ResetOtherImages(imageC);
        //}
        /*
        switch (gameManager.PlayerChoice)
        {
            case Choice.None:
                ResetAllImages();               
                break;

            case Choice.Rock:
                ScaleImage(imageZ);
                ResetOtherImages(imageZ);
                // ���� ���ÿ� ���� ���� ó��
                break;

            case Choice.Scissors:                
                ScaleImage(imageX);
                ResetOtherImages(imageX);
                // �� ���ÿ� ���� ���� ó��
                break;

            case Choice.Paper:                
                ScaleImage(imageC);
                ResetOtherImages(imageC);
                // ���� ���ÿ� ���� ���� ó��
                break;

            case Choice.TimeOut:
                Debug.Log("�÷��̾� �Է� �ð��� �ʰ��Ǿ����ϴ�.");
                // �ð� �ʰ��� ���� ���� ó��
                break;

            default:
                Debug.LogError("�� �� ���� �����Դϴ�.");
                break;
        }
        */

    }

    private void ScaleImage(RectTransform image)
    {
        image.DOScale(originalScale * scaleMultiplier, scaleDuration).SetEase(Ease.OutQuad);
    }

    private void ResetOtherImages(RectTransform scaledImage)
    {
        if (scaledImage != imageZ) imageZ.DOScale(originalScale, scaleDuration);
        if (scaledImage != imageX) imageX.DOScale(originalScale, scaleDuration);
        if (scaledImage != imageC) imageC.DOScale(originalScale, scaleDuration);
    }

    public void ResetAllImages()    //ui������ �ʱ�ȭ //�������� ��ȯ�� �ѹ� ȣ��
    {
        imageZ.DOScale(originalScale, scaleDuration);
        imageX.DOScale(originalScale, scaleDuration);
        imageC.DOScale(originalScale, scaleDuration);
    }

    public void ChoiceThat(Choice choice)
    {
        switch (choice)
        {
            case Choice.None:
                ResetAllImages();
                break;

            case Choice.Rock:
                ScaleImage(imageZ);
                ResetOtherImages(imageZ);
                // ���� ���ÿ� ���� ���� ó��
                break;

            case Choice.Scissors:
                ScaleImage(imageX);
                ResetOtherImages(imageX);
                // �� ���ÿ� ���� ���� ó��
                break;

            case Choice.Paper:
                ScaleImage(imageC);
                ResetOtherImages(imageC);
                // ���� ���ÿ� ���� ���� ó��
                break;

            case Choice.TimeOut:
                Debug.Log("�÷��̾� �Է� �ð��� �ʰ��Ǿ����ϴ�.");
                // �ð� �ʰ��� ���� ���� ó��
                break;

            default:
                Debug.LogError("�� �� ���� �����Դϴ�.");
                break;
        }
    }


}