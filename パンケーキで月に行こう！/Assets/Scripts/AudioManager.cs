using UnityEngine;

public enum StageAudioType
{
    Ground,
    Cloud,
    Space,
    Moon
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM")]
    public AudioClip groundBgm;
    public AudioClip cloudBgm;
    public AudioClip spaceBgm;
    public AudioClip moonBgm;

    [Range(0f, 1f)]
    public float bgmVolume = 0.35f;

    [Header("Sound Effects")]
    public AudioClip jumpSe;
    public AudioClip stageChangeSe;
    public AudioClip lifeZeroSe;
    public AudioClip damageSe;
    public AudioClip buttonClickSe;
    public AudioClip clearSe;
    public AudioClip gameOverSe;

    [Range(0f, 1f)]
    public float seVolume = 0.8f;

    private AudioSource bgmSource;
    private AudioSource seSource;

    private bool hasCurrentStage = false;
    private StageAudioType currentStage;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = bgmVolume;

        seSource = gameObject.AddComponent<AudioSource>();
        seSource.loop = false;
        seSource.playOnAwake = false;
        seSource.volume = seVolume;
    }

    public void PlayStageBgm(StageAudioType stage, bool playStageChangeSe = true)
    {
        if (hasCurrentStage && currentStage == stage)
        {
            return;
        }

        bool isFirstStage = !hasCurrentStage;

        hasCurrentStage = true;
        currentStage = stage;

        AudioClip nextClip = GetBgmClip(stage);

        if (nextClip != null && bgmSource.clip != nextClip)
        {
            bgmSource.clip = nextClip;
            bgmSource.volume = bgmVolume;
            bgmSource.Play();
        }

        if (!isFirstStage && playStageChangeSe)
        {
            PlayStageChange();
        }
    }

    public void StopBgm()
    {
        if (bgmSource != null)
        {
            bgmSource.Stop();
        }

        hasCurrentStage = false;
    }

    public void PlayJump()
    {
        PlaySe(jumpSe);
    }

    public void PlayStageChange()
    {
        PlaySe(stageChangeSe);
    }

    public void PlayLifeZero()
    {
        PlaySe(lifeZeroSe);
    }

    public void PlayDamage()
    {
        PlaySe(damageSe);
    }

    public void PlayButtonClick()
    {
        PlaySe(buttonClickSe);
    }

    public void PlayClear()
    {
        PlaySe(clearSe);
    }

    public void PlayGameOver()
    {
        PlaySe(gameOverSe);
    }

    private AudioClip GetBgmClip(StageAudioType stage)
    {
        switch (stage)
        {
            case StageAudioType.Ground:
                return groundBgm;

            case StageAudioType.Cloud:
                return cloudBgm;

            case StageAudioType.Space:
                return spaceBgm;

            case StageAudioType.Moon:
                return moonBgm;

            default:
                return null;
        }
    }

    private void PlaySe(AudioClip clip)
    {
        if (clip == null || seSource == null)
        {
            return;
        }

        seSource.PlayOneShot(clip, seVolume);
    }
}