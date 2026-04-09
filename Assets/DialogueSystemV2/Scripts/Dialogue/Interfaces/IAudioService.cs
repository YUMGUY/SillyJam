using System.Collections;
using UnityEngine;

public interface IAudioService
{
    void PlayMusic(AudioClip clip);
    void StopMusic();
    IEnumerator FadeMusicOut(float duration);
    void PlaySFX(AudioClip clip);
}