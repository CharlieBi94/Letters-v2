using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviewIcon : MonoBehaviour
{
    [SerializeField]
    Image iconImg;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Intialize());
    }

    private IEnumerator Intialize()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.NextLetterChanged += OnNextLevelChanged;
        iconImg.sprite = LetterSpriteLoader.GetSprite(GameManager.Instance.GetNextLetter());
    }

    private void OnNextLevelChanged(char c)
    {
        iconImg.sprite = LetterSpriteLoader.GetSprite(c);
    }
}
