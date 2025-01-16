using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GaugeUi : MonoBehaviour
{
    public Slider gaugeSlider;
    public float shakeDuration = 0.5f;
    public float shakeStrength = 5f;
    public int vibrato = 10;
    public float stepSize = 0.1f; // 1/10 단위로 조정

    public void IncreaseGauge()
    {
        float targetValue = Mathf.Min(gaugeSlider.value + stepSize, 1f);
        AnimateGauge(targetValue);
    }

    public void DecreaseGauge()
    {
        float targetValue = Mathf.Max(gaugeSlider.value - stepSize, 0f);
        AnimateGauge(targetValue);
    }

    private void AnimateGauge(float targetValue)
    {
        // 게이지 값을 부드럽게 변경
        gaugeSlider.DOValue(targetValue, shakeDuration).SetEase(Ease.OutQuad);

        // 게이지를 흔들기
        gaugeSlider.transform.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, 0), vibrato)
            .SetEase(Ease.OutQuad);
    }
}
