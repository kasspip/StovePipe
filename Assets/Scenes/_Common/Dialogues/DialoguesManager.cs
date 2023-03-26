using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;


public class DialoguesManager : MonoBehaviour
{
    public static DialoguesManager Singleton = null;
    private static CanvasGroup DialogueBox => Singleton._canvasGroup;
    private static TextMeshProUGUI Dialogue => Singleton._dialogue;

    public static Color AlanColor => Singleton._alanDialogueColor;
    public static Color SaadColor => Singleton._saadDialogueColor;
    public static Color RamiColor => Singleton._saadDialogueColor;
    public static Color EddyColor => Singleton._eddyDialogueColor;
    public static Color GriffColor => Singleton._griffDialogueColor;

    [Header("Setttings")]
    [SerializeField] Color _alanDialogueColor;
    [SerializeField] Color _saadDialogueColor;
    [SerializeField] Color _ramyDialogueColor;
    [SerializeField] Color _eddyDialogueColor;
    [SerializeField] Color _griffDialogueColor;

    [Header("References")]
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] TextMeshProUGUI _dialogue;

    private static Sequence _dialogueSeq = null;
    private static int _dialogueIndex = 0;
    private static (Color, string)[] _dialoguesSequence;

    private void Awake()
    {
        Singleton = this;
        _dialogueIndex = -1;
    }

    private void Start()
    {
        _canvasGroup.alpha = 0;
    }

    private void Update()
    {
    }

    public static void DisplayDialogue(string text, float duration)
    {
        if (_dialogueSeq != null)
            _dialogueSeq.Kill();

        Dialogue.text = text;

        if (duration == 0) return;

        _dialogueSeq = DOTween.Sequence();
        _dialogueSeq.Append(DialogueBox.DOFade(1, .25f));
        _dialogueSeq.AppendInterval(duration);
        _dialogueSeq.Append(DialogueBox.DOFade(0, .25f));
        _dialogueSeq.AppendCallback(() => _dialogueSeq = null);
    }

    public static void StartNewDialogueSequence((Color, string)[] lines)
    {
        _dialogueIndex = 0;
        _dialoguesSequence = lines;
        ReadNextDialogueSequence();
    }

    public static int ReadNextDialogueSequence()
    {
        if (_dialogueIndex < 0)
            return _dialogueIndex;

        if (_dialogueIndex < _dialoguesSequence.Length)
        {
            Dialogue.text = $"<color=#{ColorUtility.ToHtmlStringRGBA(_dialoguesSequence[_dialogueIndex].Item1)}>{ _dialoguesSequence[_dialogueIndex].Item2} </color>";
            if (DialogueBox.alpha == 0)
                DialogueBox.DOFade(1, .3f);
            else
                DOTween.Sequence()
                    .AppendCallback(()=> DialogueBox.alpha = 0)
                    .Append(DialogueBox.DOFade(1f, .3f));
            _dialogueIndex++;
        }
        else
        {
            DialogueBox.DOFade(0, .25f);
            DialogueBox.alpha = 0;

            _dialogueIndex = -1;
        }
        return _dialogueIndex;
    }
}
