using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("UI Sound Effects")]
    [SerializeField] private AudioClip diceRollSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip collectSound;

    [Header("Audio Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Settings")]
    [SerializeField] private float defaultVolume = 0.7f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
        }

        sfxSource.playOnAwake = false;
        sfxSource.volume = defaultVolume;
    }

    public void PlayDiceRoll()
    {
        PlaySound(diceRollSound);
    }

    public void PlayButtonClick()
    {
        PlaySound(buttonClickSound);
    }

    public void PlayCollectSound()
    {
        PlaySound(collectSound);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void SetVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
    }
}
