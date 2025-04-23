using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PreviewIcon : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject iconContainer;
    [SerializeField]
    Image[] icons;

    // Start is called before the first frame update
    void Start()
    {
        icons = iconContainer.GetComponentsInChildren<Image>(true);
        StartCoroutine(Intialize());
    }

    private IEnumerator Intialize()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.NextLetterChanged += OnNextLevelChanged;
        OnNextLevelChanged(GameManager.Instance.GetNextLetter());
    }

    private void OnNextLevelChanged(string s)
    {
        int n = s.Length;
        // For now, we only allow strings of length 1 or 2
        if (n > 2)
        {
            Debug.LogError("Cannot spawn letters greater than 2 in length");
            return;
        }
        for(int i = 0; i < icons.Length; i++)
        {
            if(i < n)
            {
                icons[i].gameObject.SetActive(true);
                icons[i].sprite = LetterSpriteLoader.GetSprite(s[i]);
            }
            else
            {
                icons[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.SpawnNextLetter();
    }
}
