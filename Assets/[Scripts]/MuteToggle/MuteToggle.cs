using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteToggle : MonoBehaviour
{
    [SerializeField]
    bool isMute;

    [SerializeField]
    Sprite unMutedSprite;
    [SerializeField]
    Sprite mutedSprite;

    Image buttonImg;

    // Start is called before the first frame update
    void Start()
    {
        buttonImg = GetComponent<Image>();
        HandleStateChange();
    }

    public void ToggleMute()
    {
        isMute = !isMute;
        SoundController.Instance.SetMute(isMute);
        HandleStateChange();
    }

    private void HandleStateChange()
    {
        if (isMute)
        {
            buttonImg.sprite = mutedSprite;
        }
        else
        {
            buttonImg.sprite = unMutedSprite;
        }
    }
}
