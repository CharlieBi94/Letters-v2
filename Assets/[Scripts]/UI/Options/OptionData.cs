using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionData : MonoBehaviour
{
    public event Action<OptionData> OptionChanged;
    public char content {get; private set;}
    public string description { get; private set;}

    public void Initialize(char c, string desc)
    {
        content = c;
        description = desc;
        OptionChanged?.Invoke(this);
    }
}
