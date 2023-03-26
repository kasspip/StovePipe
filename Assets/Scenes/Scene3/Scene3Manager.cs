using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene3Manager : Scene1Manager
{
    [SerializeField] Transform _torchLight;

    private Vector3 _mousePosition;

    protected override void Start()
    {
        AudioManager.Play("Fizz");
        HideAllSilhoutettes();
        DisplaySilhouette(0);
        _fader.gameObject.SetActive(true);
        _fader.alpha = 1;
        DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() => SetPhase(Phase.Phase1));

    }

    protected override void Update()
    {
        UpdatePhase();
        UpdateTorchLightPosition();
    }

    private void UpdateTorchLightPosition()
    {
        _mousePosition = Input.mousePosition;
        _mousePosition.z = 10;
        _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
        _mousePosition.z = 0 + Camera.main.nearClipPlane;

        _torchLight.position = _mousePosition;
    }

    #region Phase 1

    protected override void LoadPhase1()
    {
        _tuto.Display(2f, "Inspecter", "");

        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.AlanColor, "Alan: Ça siffle. Reculez !"),
        };
        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected override void HandlePhase1()
    {
        if (Input.GetMouseButtonDown(0))
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
        AudioManager.Play("FlashLight");
        _fader.gameObject.SetActive(true);
        _fader.alpha = 0;
        _tuto.Display(2f, "Fouiller", "");
        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.GriffColor, "Grif: Si ça fait ce bruit, ça veut dire qu’il y a un souci avec le détonateur."),
        };
        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected override void HandlePhase2()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            if (lineIndex == -1)
            {
                SetPhase(Phase.Phase3);
            }
        }
    }

    #endregion
    #region Phase 3

    protected override void LoadPhase3()
    {
        DisplaySilhouette(1);

        _tuto.Display(2f, "Parler", "");
        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.AlanColor, "Alan: Ah merde... P****"),
            (DialoguesManager.AlanColor, "Alan: Y a que moi qui ait tiré ? Je suis le seul ?"),
            (DialoguesManager.EddyColor, "Eddy: Ça change quelque chose ?"),
            (DialoguesManager.AlanColor, "Alan: MERDE!"),
            (DialoguesManager.EddyColor, "Eddy: Tu crois qu’il nous avait entendu ?"),
            (DialoguesManager.GriffColor, "Grif: C’est sûr que oui. Il avait dû les voler..."),

        };
        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected override void HandlePhase3()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            if (lineIndex == -1)
            {
                LoadScene4();
            }
        }
    }

    private void LoadScene4()
    {
        _tuto.Hide();
        _fader.gameObject.SetActive(true);
        _fader.alpha = 0;
        DOTween.Sequence()
            .Append(_fader.DOFade(1, 3f))
            .AppendCallback(()=> SceneManager.LoadScene("Scene4"));
    }

    #endregion
}
