using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnimationData
{
    public SimpleAnimator.AnimationType Type {  get; private set; }
    public float Duration { get; private set; }

    public Sprite Sprite { get; private set; }

    public AnimationData(SimpleAnimator.AnimationType type, float duration, Sprite target = null)
    {
        Type = type;
        if (duration > 0f) Duration = duration;
        Sprite = target;
    }
}
