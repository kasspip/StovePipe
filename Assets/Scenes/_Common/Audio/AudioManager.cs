using UnityEngine.Audio;
using UnityEngine;
using DG.Tweening;


public class AudioManager : MonoBehaviour
{
    public static AudioManager Singleton = null;

    [Header("Settings")]
    [SerializeField] AudioInfos[] _audios;
    [SerializeField] AudioMixerGroup _musicMixer;
    [SerializeField] AudioMixerGroup _FxMixer;


    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            Singleton = this;

        DontDestroyOnLoad(gameObject);

        foreach (var audio in _audios)
        {
            audio.source = gameObject.AddComponent<AudioSource>();
            audio.source.clip = audio.clip;
            audio.source.volume = audio.volume;
            audio.source.pitch = audio.pitch;
            audio.source.loop = audio.loop;

            switch (audio.mixer)
            {
                case AudioInfos.Mixer.Music: audio.source.outputAudioMixerGroup = _musicMixer; break;
                case AudioInfos.Mixer.FX: audio.source.outputAudioMixerGroup = _FxMixer; break;
            }
        }
    }

    private void Start()
    {
    }

    private void Update()
    {
    }

    public static void Play(string clipName)
    {
        foreach (var audio in Singleton._audios)
        {
            if (audio.name == clipName)
            {
                audio.source.Play();
                return;
            }
        }
        throw new UnityException($"could not find audio {clipName}");
    }

    public static void Stop(string clipName, float fadeDuration = 0)
    {
        foreach (var audio in Singleton._audios)
        {
            if (audio.name == clipName)
            {
                if (fadeDuration == 0)
                    audio.source.Stop();
                else
                    audio.source.DOFade(0, fadeDuration).SetEase(Ease.OutSine);
                return;
            }
        }
        throw new UnityException($"could not find audio {clipName}");
    }
}

[System.Serializable]
public class AudioInfos
{
    public enum Mixer
    {
        Music,
        FX
    }

    public string name;
    public AudioClip clip;
    public Mixer mixer = Mixer.Music;
    public bool loop = false;
    [Range(0,1)] public float volume = 1;
    [Range(0.1f, 3)] public float pitch = 1;
    [HideInInspector] public AudioSource source;
}

