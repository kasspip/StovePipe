using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Tuto : MonoBehaviour
{
    [SerializeField] CanvasGroup _mouseGroup;
    [SerializeField] TextMeshProUGUI _leftBtnTuto;
    [SerializeField] TextMeshProUGUI _rightBtnTuto;

    private void Awake()
    {
        Hide();
    }

    public void Display(float fadeDuration, string txtL, string txtR)
    {
        _leftBtnTuto.text = txtL;
        _rightBtnTuto.text = txtR;
        _mouseGroup.alpha = 0;

        if (fadeDuration > 0)
            _mouseGroup.DOFade(1, 1f);
        else
            _mouseGroup.alpha = 1;
    }

    public void Hide()
    {
        _leftBtnTuto.text = "";
        _rightBtnTuto.text = "";
        _mouseGroup.alpha = 0;
    }

}
