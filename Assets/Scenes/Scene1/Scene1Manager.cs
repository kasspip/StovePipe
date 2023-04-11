using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.Rendering.Universal;

public class Scene1Manager : MonoBehaviour
{
    [SerializeField] protected Phase _currentPhase = Phase.None;

    [Header("References")]
    [SerializeField] protected CanvasGroup _fader;
    [SerializeField] protected Tuto _tuto;
    [SerializeField] protected Light2D _globalLight;
    [SerializeField] protected Camera _camera;
    [SerializeField] protected GameObject[] _silhouettes;
    [SerializeField] private Animator _weaponAnimator;
    [SerializeField] private CanvasGroup _timeStamp;

    Sequence _hitTween = null;
    int _lineIndex = -1;

    public enum Phase
    {
        None,
        Phase1,
        Phase2,
        Phase3,
        Phase4,
        Phase5,
    }

    protected virtual void Awake()
    {
        _weaponAnimator.gameObject.SetActive(false);
        _timeStamp.alpha = 0;
    }

    protected virtual void Start()
    {
        _fader.alpha = 1;
        AudioManager.Play("Basement");
        HideAllSilhoutettes();
        DisplaySilhouette(0);
        DOTween.Sequence()
            .AppendInterval(1f)
            .Append(_fader.DOFade(0, 2f))
            .AppendCallback(()=> SetPhase(Phase.Phase1));
        
    }

    protected virtual void Update()
    {
        UpdatePhase();
    }

    #region Phase Handling

    protected void DisplaySilhouette(int index)
    {
        HideAllSilhoutettes();
        _silhouettes[index].SetActive(true);
    }

    protected void HideAllSilhoutettes()
    {
        for (int i = 0; i < _silhouettes.Length; i++)
            _silhouettes[i].SetActive(false);
    }

    protected void SetPhase(Phase phase)
    {
        _currentPhase = phase;
        switch (phase)
        {
            case Phase.None: break;
            case Phase.Phase1: LoadPhase1(); break;
            case Phase.Phase2: LoadPhase2(); break;
            case Phase.Phase3: LoadPhase3(); break;
            case Phase.Phase4: LoadPhase4(); break;
            case Phase.Phase5: LoadPhase5(); break;
        }
    }

    protected void UpdatePhase()
    {
        switch (_currentPhase)
        {
            case Phase.None: break;
            case Phase.Phase1: HandlePhase1(); break;
            case Phase.Phase2: HandlePhase2(); break;
            case Phase.Phase3: HandlePhase3(); break;
            case Phase.Phase4: HandlePhase4(); break;
            case Phase.Phase5: HandlePhase5(); break;
        }
    }

    #endregion
    #region Phase 1

    protected virtual void LoadPhase1()
    {
        DisplaySilhouette(0); // inconnu

        _tuto.Display(2f, "Parler", "");

        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.SaadColor, "Saad: Tu parles arabe ? "),
            (DialoguesManager.AlanColor, "Alan: Shway bes. Ga’id at’allam. [Pas beaucoup. J’essaie d’apprendre.] "),
            (DialoguesManager.SaadColor, "Saad: Tu parles bien ! "),
            (DialoguesManager.AlanColor, "Alan: Non, pas vraiment. "),
            (DialoguesManager.SaadColor, "Saad: Non, bien sûr que non, c’est pas bien. Je te flatte. "),
            (DialoguesManager.AlanColor, "Alan: (...) "),
            (DialoguesManager.SaadColor, "Saad: T’as appris où, à parler mal arabe?"),
            (DialoguesManager.AlanColor, "*Click*"),
            (DialoguesManager.AlanColor, "Alan: INZIL!"),
        };
        DialoguesManager.StartNewDialogueSequence(lines);
        DOTween.Sequence()
            .AppendInterval(1f)
            .Append(_timeStamp.DOFade(1, 3f))
            .Append(_timeStamp.DOFade(0, 2f));
    }

    protected virtual void HandlePhase1()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();

            if (lineIndex == 8)
            {
                AudioManager.Play("GunReload");
            }
            if (lineIndex == 9)
            {
                AudioManager.Stop("Basement", 0.5f);
                AudioManager.Play("Inzil");

            }
            if (lineIndex == -1)
            {
                SetPhase(Phase.Phase2);
            }
        }
    }

    #endregion
    #region Phase2

    protected virtual void LoadPhase2()
    {
        DisplaySilhouette(1); // Rami
        _tuto.Display(2f, "Parler", "Frapper");

        _weaponAnimator.gameObject.SetActive(true);
        _weaponAnimator.Play("rotate_up", 0);

        StartCoroutine(LightFade(1, 0, 1));
        _camera.DOColor(new Color(.17f, .2f, .26f, 1), .7f);

        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.AlanColor, "Alan: Inzil, inzil ‘al-ardh! [A genoux! A genoux à terre!]"),
            (DialoguesManager.RamiColor, "???: Ma sawwait shei! [J’ai rien fait !]"),
        };

        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected virtual void HandlePhase2()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            if (lineIndex == -1)
            {
                SetPhase(Phase.Phase3);
            }

        }
        if (Input.GetMouseButtonDown(1))
        {
            Hit(_silhouettes[1].transform);
            _weaponAnimator.SetTrigger("isKicking");
        }
    }

    protected IEnumerator LightFade(float duration, float from, float to)
    {
        _globalLight.intensity = from;
        float elapsedtime = 0;
        while(elapsedtime < duration)
        {
            elapsedtime += Time.deltaTime;
            _globalLight.intensity = elapsedtime * 1 / to;
            yield return new WaitForEndOfFrame();
        }
        _globalLight.intensity = to;
    }

    #endregion
    #region Phase 3

    protected virtual void LoadPhase3()
    {


        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.AlanColor, "Alan: Ithan la t-gawim. Ma ereed e’awrak. [Alors arrête de résister. Je ne veux pas te faire de mal.]"),
            (DialoguesManager.AlanColor, "INZIL! [A terre!]"),
            (DialoguesManager.AlanColor, "Alan: La t-goom illa lemma egillak. [C’est moi qui te dirai quand te relever.]"),
        };

        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected virtual void HandlePhase3()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            if (lineIndex == -1)
            {
                SetPhase(Phase.Phase4);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Hit(_silhouettes[1].transform);
            _weaponAnimator.SetTrigger("isKicking");
        }
    }

    protected virtual void Hit(Transform t)
    {
        var rand = Random.Range(0f, 1f);
        if (rand > .5f)
            AudioManager.Play("Grunt1");
        else
            AudioManager.Play("Grunt2");

        if (_hitTween != null)
            _hitTween.Kill();

        Vector3 originScale = t.localScale;
        _hitTween = DOTween.Sequence();
        _hitTween.Append(t.DOScale(originScale - new Vector3(.05f, .05f, .05f), 0.1f));
        _hitTween.Append(t.DOScale(originScale, 0.2f));
    }

    #endregion
    #region Phase 4

    protected virtual void LoadPhase4()
    {
        DisplaySilhouette(2); // Rami genou 

        _tuto.Display(2f, "Continuer", "");
        AudioManager.Stop("Inzil", 2f);
        AudioManager.Play("Basement");


        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.AlanColor, "..."),
            (DialoguesManager.AlanColor, "Alan: C’est bien, là, non ?"),
            (DialoguesManager.RamiColor, "???: Ouais, ça va, c’est pas mal."),
        };

        DialoguesManager.StartNewDialogueSequence(lines);
    }

    protected virtual void HandlePhase4()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var lineIndex = DialoguesManager.ReadNextDialogueSequence();
            if (lineIndex == 1)
                AudioManager.Play("Basement");
            if (lineIndex == -1)
            {
                SetPhase(Phase.Phase5);
            }
        }
    }

    #endregion
    #region Phase 5

    protected virtual void LoadPhase5()
    {
        DisplaySilhouette(3); // rami se releve
        _weaponAnimator.Play("rotate_down", 0);

        _tuto.Display(2f, "Continuer", "");

        (Color, string)[] lines = new (Color, string)[]
        {
            (DialoguesManager.RamiColor, "Rami: OK, alors Alan, dans l’ensemble, c’était bien."),
            (DialoguesManager.RamiColor, "Rami: Vous voyez, les gars ? Le positionnement était bien."),
            (DialoguesManager.RamiColor, "Rami: L’arme, peut-être pas sur la joue, mais physiquement, c’était solide."),
            (DialoguesManager.RamiColor, "Rami: Pour dominer l’interaction, il faut absolument parler arabe."),
            (DialoguesManager.RamiColor, "Rami: Ils ne s’attendront pas à ce que vous le parliez."),
            (DialoguesManager.RamiColor, "Rami: L’accent d’Alan est pas ouf, mais juste le fait qu’il ait parlé arabe, j’ai trouvé ça percutant."),
            (DialoguesManager.RamiColor, "Rami: On t’a appris ça à Basra ?"),
            (DialoguesManager.AlanColor, "Alan: Ouais… Le corps et l’esprit, tout ça…"),
            (DialoguesManager.RamiColor, "Rami: OK… Bon les gars, c’est la dernière fois qu’on fait le point avant le déploiement"),
            (DialoguesManager.RamiColor, "Rami: Ça fait pas long comme entraînement, mais on compte sur vous pour gérer."),
            (DialoguesManager.RamiColor, "Rami: À votre avis, dans le véhicule, on a besoin de quoi à part les munitions et le matériel de secours ?"),
            (DialoguesManager.AlanColor, "Alan: Une meuf."),
            (DialoguesManager.EddyColor, "Eddy: A boire."),
            (DialoguesManager.GriffColor, "Grif: Canal+"),
            (DialoguesManager.RamiColor, "Rami: ... J’espère que vous trouverez ça toujours aussi marrant dans quelques mois ..."),
        };

        DialoguesManager.StartNewDialogueSequence(lines);

        _timeStamp.GetComponent<TextMeshProUGUI>().text = "Irak 2003";
        DOTween.Sequence()
            .AppendInterval(1f)
            .Append(_timeStamp.DOFade(1, 3f))
            .Append(_timeStamp.DOFade(0, 2f));
    }

    protected virtual void HandlePhase5()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_lineIndex < 15)
            {
                _lineIndex = DialoguesManager.ReadNextDialogueSequence();

                if (_lineIndex == 11)
                {
                    _fader.DOFade(1, 5);
                }
            }
            else
            {
                StartCoroutine(NextScene());
            }
        }
    }

    #endregion

    private IEnumerator NextScene()
    {
        DialoguesManager.ReadNextDialogueSequence();
        AudioManager.Stop("Basement", 3f);

        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Scene2");
    }
}
