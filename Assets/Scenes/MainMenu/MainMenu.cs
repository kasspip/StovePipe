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
    [SerializeField] CanvasGroup _disclaimer;

    protected void Start()
    {
        _fader.alpha = 0;
        _disclaimer.alpha = 0;
        HideTutorial();
        HideCredits();
        AudioManager.Play("Inzil");
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
        AudioManager.Stop("Inzil", 10f);

        DOTween.Sequence()
        .Append(_fader.DOFade(1, 1f))
        .Append(_disclaimer.DOFade(1, .5f))
        .AppendInterval(10f)
        .Append(_disclaimer.DOFade(0, .5f))
        .AppendCallback(() => SceneManager.LoadScene("Scene1"));
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
