using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXEffect : MonoBehaviour
{
    [SerializeField]
    AudioClip soundEffect;

    public void PlaySFX()
    {
        SoundController.Instance.PlaySoundEffect(soundEffect);
    }
}
