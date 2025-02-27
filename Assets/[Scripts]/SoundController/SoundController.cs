using UnityEngine;

public class SoundController : Singleton<SoundController>
{

    [SerializeField]
    private AudioClip backgroundAudio;

    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
        PlayAudio(backgroundAudio, true);
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }

    public void SetVolume(float amount)
    {
        audioSource.volume = Mathf.Clamp01(amount);

    }

    public bool GetMute()
    {
        return audioSource.mute;
    }

    public void SetMute(bool isMute)
    {
        audioSource.mute = isMute;
    }

    public void PlayAudio(AudioClip clip, bool isLooping = false)
    {
        if (isLooping)
        {
            audioSource.clip = clip;
            audioSource.loop = isLooping;
            audioSource.Play();
        }
        else
        {
            audioSource.PlayOneShot(clip);
        }

    }
}
