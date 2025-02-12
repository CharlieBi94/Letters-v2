using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WildcardOptions : MonoBehaviour
{
    [SerializeField]
    GameObject optionPrefab;

    [SerializeField]
    RectTransform tileContainer;

    List<GameObject> options = new();

    [SerializeField]
    GameObject visibleContainer;
    private void Start()
    {
        if(options.Count == 0)
        {
            if (!visibleContainer.activeInHierarchy)
            {
                visibleContainer.SetActive(true);
            }
            for (int i = 97; i <= 122; i++)
            {
                char c = (char)i;
                var obj = Instantiate(optionPrefab);
                obj.transform.SetParent(tileContainer, false);
                obj.GetComponent<OptionData>().Initialize(c, "");
                options.Add(obj);
            }
        }
        if (visibleContainer.activeInHierarchy)
        {
            visibleContainer.SetActive(false);
        }
    }

    public void ShowOptions(Tile targetTile)
    {
        foreach(var obj in options)
        {
            char c = obj.GetComponent<OptionData>().content;
            obj.GetComponent<Button>().onClick.RemoveAllListeners();
            obj.GetComponent<Button>().onClick.AddListener(() => ChangeLetter(c, targetTile));
        }
        visibleContainer.SetActive(true);
    }

    public void ChangeLetter(char c, Tile targetTile)
    {
        targetTile.SetChar(c);
        targetTile.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        HideOptions();
    }

    public void HideOptions()
    {
        visibleContainer.SetActive(false);
    }
}
