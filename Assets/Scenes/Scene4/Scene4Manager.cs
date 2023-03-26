using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene4Manager : Scene1Manager
{
    [SerializeField] Transform _torchLight;
    [SerializeField] GameObject _drinks;
    [SerializeField] CanvasGroup _end;
    [SerializeField] CanvasGroup _credits;

    private bool _isTheEnd = false;

    private Vector3 _mousePosition;

    protected override void Start()
    {
        _end.gameObject.SetActive(false);
        _credits.gameObject.SetActive(false);

        _fader.gameObject.SetActive(true);
        _fader.DOFade(0, 2f);
        DOTween.Sequence()
            .AppendInterval(2f)
            .AppendCallback(() => _drinks.gameObject.SetActive(true))
            .AppendInterval(1f)
            .AppendCallback(() => SetPhase(Phase.Phase1));

    }

    protected override void Update()
    {
        UpdatePhase();
    }



    #region Phase 1

    protected override void LoadPhase1()
    {
        _tuto.Display(2f, "Pepsi", "Coca");
        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.EddyColor, "Eddy: T’es plutôt Coca ou Pepsi ?"),
        };
        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected override void HandlePhase1()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            if (lineIndex == -1)
            {
                SetPhase(Phase.Phase2);
            }
        }
    }

    #endregion
    #region Phase 2

    protected override void LoadPhase2()
    {
        _tuto.Hide();
        DOTween.Sequence()
            .Append(_fader.DOFade(1, 4f))
            .AppendCallback(() => ShowEnd());
    }

    protected override void HandlePhase2()
    {
        if (Input.GetMouseButtonDown(0) && _isTheEnd)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    private void ShowEnd()
    {
        _end.gameObject.SetActive(true);
        _end.alpha = 0;
        DOTween.Sequence()
            .Append(_end.DOFade(1, 3f))
            .AppendInterval(3f)
            .Append(_end.DOFade(0, 2f))
            .AppendCallback(() => ShowCredits());
    }

    private void ShowCredits()
    {
        _end.gameObject.SetActive(false);
        _credits.gameObject.SetActive(true);

        _credits.alpha = 0;
        _credits.DOFade(1, 3);
        _isTheEnd = true;
        DOTween.Sequence()
            .AppendInterval(4f)
            .AppendCallback(() => _tuto.Display(2f, "Quitter", ""));
    }

    #endregion
}
