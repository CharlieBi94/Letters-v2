using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{ 
    [SerializeField]
    TextMeshProUGUI tmpDisplay;

    private void Start()
    {
        ScoreData.Instance.ScoreChanged += OnScoreChanged;
        OnScoreChanged();
    }

    private void OnScoreChanged()
    {
        tmpDisplay.text = ScoreData.Instance.GetScore().ToString();
    }

    private void OnEnable()
    {
        tmpDisplay.text = ScoreData.Instance.GetScore().ToString();
    }
}
