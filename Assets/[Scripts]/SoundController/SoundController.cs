using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundController : Singleton<SoundController>
{

    [SerializeField]
    public AudioClip menuClip;
    [SerializeField]
    public AudioClip gameClip;
    [SerializeField]
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneChange;
        OnSceneChange(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    private void OnSceneChange(Scene scene, LoadSceneMode arg1)
    {
        if(scene.buildIndex == 0)
        {
            PlayAudio(menuClip, true);
        }
        else
        {
            PlayAudio(gameClip, true);
        }
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
        audioSource.clip = clip;
        audioSource.loop = isLooping;
        audioSource.Play();
    }

    public void PlaySoundEffect(AudioClip clip, float volumeScale = 1f)
    {
        audioSource.PlayOneShot(clip, volumeScale);
    }

    public AudioClip GetCurrentClip()
    {
        return audioSource.clip;
    }
}
