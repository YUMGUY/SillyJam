using System.Collections;
using UnityEngine;

public class GeneralAudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    void Start()
    {
        
    }

    public void StartFadeOut(float duration)
    {
        StartCoroutine(FadeMusic(duration));
    }

    public IEnumerator FadeMusic(float duration)
    {
        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.volume = startVolume;

        yield return null;
    }
}
