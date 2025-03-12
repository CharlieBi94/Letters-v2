using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static OptionsWindow.OPTION_TYPE;

public class OptionsGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject optionPrefab;
    [SerializeField]
    RectTransform uiCanvasRect;
    [SerializeField]
    RectTransform rect;
    [SerializeField]
    TextMeshProUGUI tmp;
    private readonly List<GameObject> options = new();
    
    // Generates the specified number of options in the swap screen
    private void GenerateOptions(int amount)
    {
        options.Clear();
        GetComponentsInChildren<OptionData>(true).ToList().ForEach(data => { options.Add(data.gameObject); });
        if (options.Count == amount) { return; }
        else if(options.Count > amount)
        {
            int deleteCount = options.Count - amount;
            for (int i = 0, n = deleteCount; i < n; i++)
            {
                DestroyOption(options[options.Count-1]);
            }
        }
        else if (options.Count < amount)
        {
            for (int i = 0, n = amount - options.Count; i < n; i++)
            {
                CreateOption();
            }
        }

        options.ForEach(option => { option.GetComponent<Button>().onClick.RemoveAllListeners(); });
        
    }

    private bool CreateOption()
    {
        var obj = Instantiate(optionPrefab);
        obj.transform.SetParent(rect, false);
        options.Add(obj);
        return true;
    }

    private bool DestroyOption(GameObject option)
    {
        options.Remove(option);
        Destroy(option);
        return true;
    }

    public void SetInstructions(string instructions)
    {
        tmp.text = instructions;
    }

    public void PopulateOptions(List<char> dataContents, List<OptionsWindow.OPTION_TYPE> optionType)
    {
        if(dataContents.Count != optionType.Count)
        {
            Debug.LogError("Tried to generate options without matching number of behaviours");
            return;
        }

        GenerateOptions(dataContents.Count);

        for (int i = 0; i < options.Count; i++)
        {
            if(i < dataContents.Count)
            {
                options[i].SetActive(true);
                string desc = TooltipGenerator.GenerateTooltip(dataContents[i]);
                options[i].GetComponent<OptionData>().Initialize(dataContents[i], desc);
                OptionBehaviour behaviour = options[i].GetComponent<OptionBehaviour>();
                Button optionButton = options[i].GetComponent<Button>();
                optionButton.onClick.RemoveAllListeners();
                behaviour.SetUICanvas(uiCanvasRect);
                //if (optionType[i] == SWAP)
                //{
                //    optionButton.onClick.AddListener(behaviour.HandleSwap);
                //}else if (optionType[i] == MIDDLE_PLACEMENT)
                //{
                //    optionButton.onClick.AddListener(behaviour.HandleMiddleUpgrade);
                //}
                //else if (optionType[i] == SWAP_CONFIRM)
                //{
                //    optionButton.onClick.AddListener(behaviour.HandleConfirm);
                //}else if (optionType[i] == MIDDLE_UPGRADE_CONFIRM)
                //{
                //    optionButton.onClick.AddListener(behaviour.HandleMiddleUpgradeConfirm);
                //}else if (optionType[i] == GOD_MODE)
                //{
                //    optionButton.onClick.AddListener(behaviour.HandleGodModeSelect);
                //}else if (optionType[i] == WILD_CARD)
                //{
                //    optionButton.onClick.AddListener(behaviour.HandleWildCardSelect);
                //}
            }
            else
            {
                options[i].SetActive(false);
            }
        }
    }

    public int GetOptionsCount()
    {
        return options.Count;
    }
}
