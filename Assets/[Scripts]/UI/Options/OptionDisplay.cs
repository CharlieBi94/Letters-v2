using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionDisplay : MonoBehaviour, IUICollider
{
    [SerializeField]
    Image imgIcon;
    [SerializeField]
    TextMeshProUGUI tmpDescription;

    public event Action SizeChanged;

    // Start is called before the first frame update
    void Start()
    {
        OptionData data = GetComponent<OptionData>();
        OnOptionChanged(data);
        data.OptionChanged += OnOptionChanged;
        InvokeSizeChanged();
    }

    private void Update()
    {
        InvokeSizeChanged();
    }

    private void OnOptionChanged(OptionData optionData)
    {
        InvokeSizeChanged();
        if (optionData == null) return;
        imgIcon.sprite = LetterSpriteLoader.GetSprite(optionData.content);
        if (tmpDescription == null) return;
        tmpDescription.text = optionData.description;
    }

    public void InvokeSizeChanged()
    {
        SizeChanged?.Invoke();
    }
}
