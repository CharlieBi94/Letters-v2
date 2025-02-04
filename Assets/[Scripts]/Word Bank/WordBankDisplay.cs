using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WordBankDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tmp;

    private void OnEnable()
    {
        List<string> words = WordBank.Instance.GetWordsSubmitted();
        string format = "";
        foreach(string word in words)
        {
            format += string.Format($"{word}, ");
        }
        tmp.text = format;
    }
}
