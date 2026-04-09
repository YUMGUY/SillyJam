using System.Collections;
using UnityEngine;

public class DialogueAudioController : MonoBehaviour, IAudioService
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private void Awake()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        musicSource = sources[0];  // first AudioSource = music
        sfxSource = sources[1];  // second AudioSource = sfx

        //Debug.Log(sfxSource.priority); // testing
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip) return; // already playing this clip

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
        musicSource.clip = null;
    }

    public IEnumerator FadeMusicOut(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        StopMusic();
        musicSource.volume = startVolume; // reset volume for next time
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }
}