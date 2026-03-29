using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource characterSource;
    [Header("In Game")]
    public AudioClip backgroundMusic;
    public AudioClip hitNote;
    public AudioClip missNote;
    public AudioClip correctChoice;
    public AudioClip incorrectChoice;
    public AudioClip characterVoice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
    }
    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
        GameManager.Instance.audioManager = this;

    }


    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    public void PlayHit() => sfxSource.PlayOneShot(hitNote);
    public void PlayMiss() => sfxSource.PlayOneShot(missNote);
    public void PlayCorrect() => sfxSource.PlayOneShot(correctChoice);
    public void PlayIncorrect() => sfxSource.PlayOneShot(incorrectChoice);

    public void PlayCharacterVoice() => characterSource.PlayOneShot(characterVoice);
}
