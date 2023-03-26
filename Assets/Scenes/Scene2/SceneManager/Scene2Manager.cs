using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Scene2Manager : Scene1Manager
{
    public static Scene2Manager Singleton = null;
    public static People MouseOverPeople { get; set; } = null;

    [SerializeField] Transform _torchLight;
    [SerializeField] Color _cameraBackgroundColor;
    [SerializeField] float _globalLightValue = 0.03f;
    [SerializeField] People[] _peoples;

    [Header("Scene 2")]
    [SerializeField] GameObject _inzilPrefab;
    [SerializeField] GameObject _yamshiPrefab;
    [SerializeField] Transform _inzilRoot;
    [SerializeField] GameObject _victim;
    [SerializeField] Animator _victimAnimator;
    [SerializeField] GameObject _gunFire;
    [SerializeField] Color _cameraBackgroundColorDanger;
    [SerializeField] GameObject _timer;

    private Vector3 _mousePosition;
    private bool _enableFlashLight = false;
    Sequence _ShootSeq = null;
    private bool _canShoot = false;
    private bool _hasFired = false;

    protected override void Awake()
    {
        Singleton = this;
        _gunFire.SetActive(false);
        _timer.SetActive(false);
    }

    protected override void Start()
    {
        _fader.gameObject.SetActive(true);
        _fader.alpha = 1;
        HideAllSilhoutettes();
        DisplaySilhouette(0);
        _globalLight.intensity = _globalLightValue;
        DisableFlashLight();
        DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() => SetPhase(Phase.Phase1));

    }

    protected override void Update()
    {
        UpdatePhase();
        if (_enableFlashLight)
            UpdateTorchLightPosition();
    }

    #region FlashLight

    private void EnableFlashLight()
    {
        _torchLight.gameObject.SetActive(true);
        _enableFlashLight = true;
        AudioManager.Play("FlashLight");
        AudioManager.Play("Crowd");
    }

    private void DisableFlashLight()
    {
        _torchLight.gameObject.SetActive(false);
    }

    private void UpdateTorchLightPosition()
    {
        _mousePosition = Input.mousePosition;
        _mousePosition.z = 10;
        _mousePosition = Camera.main.ScreenToWorldPoint(_mousePosition);
        _mousePosition.z = 0 + Camera.main.nearClipPlane;

        _torchLight.position = _mousePosition;
    }

    #endregion
    #region Phase 1

    protected override void LoadPhase1()
    {
        _fader.alpha = 0;
        EnableFlashLight();
        _tuto.Display(2f, "Parler", "Frapper");
        _camera.DOColor(_cameraBackgroundColor, 5f);

        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.EddyColor, "Eddy: Imshi! Imshi! [Cassez vous !]"),
            (DialoguesManager.AlanColor, "Alan: P**** mais d’où ils sortent, tous !"),
            (DialoguesManager.EddyColor, "Eddy: Irja’ [Reculez!]"),
            (DialoguesManager.AlanColor, "Alan: C’est une manif ?"),
            (DialoguesManager.GriffColor, $"Grif: De toute façon, c’est le couvre-feu. Ils sont pas censés être là.{Environment.NewLine} Comment on dit couvre-feu en arabe ?"),
            (DialoguesManager.EddyColor, "Eddy: Al, c’est quoi couvre-feu en arabe ?"),
            (DialoguesManager.AlanColor, "Alan: J’ai une gueule de dico d’arabe ?"),
            (DialoguesManager.GriffColor, "Grif: On s’en fout. Faut qu’ils dégagent."),
            (DialoguesManager.AlanColor, "Alan: Dis-lui de se calmer..."),
            (DialoguesManager.EddyColor, "Eddy: Grif, calme-toi !"),
            (DialoguesManager.AlanColor, "Alan: Imshi! [Partez !]"),
        };
        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected override void HandlePhase1()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            if (MouseOverPeople != null)
                MouseOverPeople.GoBackHome();

            if (lineIndex == -1)
            {
                SetPhase(Phase.Phase2);
            }
            else
            {
                if (lineIndex == 8 && EverybodyIsHome())
                    SetPhase(Phase.Phase2);

                if ( lineIndex < 10)
                    DisplayInzil(_yamshiPrefab);
                if (lineIndex == 11)
                {
                    DisplayInzil(_yamshiPrefab);
                    RemoveExtraPeople();
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Hit(MouseOverPeople.transform);
            _silhouettes[0].GetComponent<Animator>().SetTrigger("isKicking");
        }
    }

    private bool EverybodyIsHome()
    {
        bool result = true;

        foreach (var people in _peoples)
        {
            if (!people.isHome)
                return false;
        }

        return result;
    }

    private void DisplayInzil(GameObject prefab)
    {
        GameObject go = Instantiate(prefab, _inzilRoot);
        go.transform.position = Input.mousePosition;

        CanvasGroup cg = go.GetComponent<CanvasGroup>();
        cg.alpha = 0;
        DOTween.Sequence()
            .Append(cg.DOFade(1, .3f))
            .Append(cg.DOFade(0, .5f))
            .AppendInterval(1f)
            .AppendCallback(()=>Destroy(go));

    }

    private void RemoveExtraPeople()
    {
        foreach (var people in _peoples)
        {
            if (!people.isHome)
            {
                people.GoBackHome();
            }
        }
    }

    #endregion
    #region Phase 2

    protected override void LoadPhase2()
    {
        AudioManager.Stop("Crowd", 3);
        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.AlanColor, "Alan: Ils veulent qu’on finisse de construire le commissariat, non ?"),
            (DialoguesManager.EddyColor, "Eddy: J’en sais rien..."),
            (DialoguesManager.EddyColor, "Grif: D’où tu me dis de m’calmer alors qu’on a une foule de locaux à gérer dans le noir total ?"),
        };
        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected override void HandlePhase2()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            //if (lineIndex == 3)
            //{
            //    _victim.SetActive(true);
            //}
            if (lineIndex == -1)
            {
                SetPhase(Phase.Phase3);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            _victim.SetActive(true);
            SetPhase(Phase.Phase3);
        }
    }

    #endregion
    #region Phase3

    protected override void LoadPhase3()
    {
        _globalLight.intensity = 0.8f;
        _victim.SetActive(true);
        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.AlanColor, "Alan: Eh!"),
            (DialoguesManager.AlanColor, "Alan: WA-GIFF MEHALLAK! [Eh, reste là !]"),
            (DialoguesManager.AlanColor, "INZIL! [A genoux !]"),
        };
        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected override void HandlePhase3()
    {
        if (Input.GetMouseButtonDown(0) && !_hasFired)
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            if (lineIndex == 2)
            {
                DisplaySilhouette(1);
                _tuto.Display(0f, "Parler", "Tirer");
                AudioManager.Play("Chase");
                _canShoot = true;
            }

            if (lineIndex == -1)
            {
                SetPhase(Phase.Phase4);
            }
        }
        if (_canShoot && Input.GetMouseButtonDown(1))
        {
            GunFire();
        }
    }

    #endregion
    #region Phase 4

    protected override void LoadPhase4()
    {
        _victimAnimator.SetBool("isRunning", true);
        _camera.DOColor(_cameraBackgroundColorDanger, 3f);
        _timer.SetActive(true);
        _timer.GetComponent<CanvasGroup>().DOFade(1, 2f);

        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.AlanColor, "Alan: WA-GIFF MAKENEK! [On ne bouge plus ! / Stop!]"),
            (DialoguesManager.AlanColor, "Alan: Attention !"),
        };
        DialoguesManager.StartNewDialogueSequence(lines);
        _ShootSeq =  DOTween.Sequence()
            .AppendInterval(5)
            .AppendCallback(()=>GunFire());
    }

    protected override void HandlePhase4()
    {
        if (Input.GetMouseButtonDown(0) && !_hasFired)
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            if (lineIndex == -1)
            {
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            _ShootSeq.Kill();
            GunFire();
        }
    }

    protected void GunFire()
    {
        _hasFired = true;
        _timer.SetActive(false);
        _fader.alpha = 1;
        _gunFire.SetActive(true);
        AudioManager.Play("GunShot");
        AudioManager.Stop("Chase",.1f);
        DOTween.Sequence()
            .AppendInterval(4f)
            .AppendCallback(()=> LoadScene3());
    }

    private void LoadScene3()
    {
        SceneManager.LoadScene("Scene3");
    }

    #endregion
}
