using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{ 
    public void _ButtonPlay() => Play();
    public void _ButtonQuit() => Quit();
    public void _ButtonDisplayTutorial() => DisplayTutorial();
    public void _ButtonHideTutorial() => HideTutorial();
    public void _ButtonDisplayCredits() => DisplayCredits();
    public void _ButtonHideCredits() => HideCredits();

    [SerializeField] GameObject _tutorialPanel;
    [SerializeField] GameObject _creditPanel;
    [SerializeField] CanvasGroup _fader;

    protected void Start()
    {
        _fader.alpha = 0;
        HideTutorial();
        HideCredits();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Quit();
    }

    #region @Play

    private void Play()
    {
        _fader.blocksRaycasts = true;
        _fader.interactable = true;

        var seq = DOTween.Sequence();
        seq.Append(_fader.DOFade(1, 1f));
        seq.AppendCallback(() => SceneManager.LoadScene("Scene1"));
    }

    #endregion
    #region @Quit

    private void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    #endregion
    #region @Tutorial

    private void DisplayTutorial()
    {
        _tutorialPanel.SetActive(true);
    }

    private void HideTutorial()
    {
        _tutorialPanel.SetActive(false);
    }

    #endregion
    #region @Credits

    private void DisplayCredits()
    {
        _creditPanel.SetActive(true);
    }

    private void HideCredits()
    {
        _creditPanel.SetActive(false);
    }

    #endregion
}
