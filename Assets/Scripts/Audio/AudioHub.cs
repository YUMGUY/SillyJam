using UnityEngine;

public class AudioHub : MonoBehaviour
{
    public static AudioHub Instance { get; private set; }
    [SerializeField] private AudioSource sfxGameSource;
    [SerializeField] private AudioClip beatMissedSFX;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (sfxGameSource == null) GetComponent<AudioSource>();
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxGameSource.PlayOneShot(clip);
    }
    public void PlayBeatMissed()
    {
        sfxGameSource.PlayOneShot(beatMissedSFX);
    }
}
