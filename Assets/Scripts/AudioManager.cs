using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("In Game")]
    public AudioClip backgroundMusic;
    public AudioClip hitNote;
    public AudioClip missNote;
    public AudioClip correctChoice;
    public AudioClip incorrectChoice;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
}
