using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreviewIcon : MonoBehaviour, IPointerClickHandler
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

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SpawnNextLetter();
    }
}
