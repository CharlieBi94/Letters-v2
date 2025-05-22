using System.Collections.Generic;
using UnityEngine;

public class OptionsWindow : MonoBehaviour
{
    [SerializeField]
    GameObject optionsWindow;
    [SerializeField]
    GameObject confirmWindow;
    [SerializeField]
    OptionsGenerator swapGenerator;
    [SerializeField]
    OptionsGenerator confirmGenerator;
    int optionsNumber = 3;
    [SerializeField]
    PlayAreaController playArea;
    [SerializeField]
    AudioClip windowOpeningAudio;
    private DifficultySettingsSO difficultySettings;

    public enum OPTION_TYPE { SWAP, MIDDLE_PLACEMENT, SWAP_CONFIRM, MIDDLE_UPGRADE_CONFIRM, GOD_MODE, WILD_CARD, WILD_CARD_OPTIONS}

    // Start is called before the first frame update
    void Start()
    {
        HideWindows();
        SwapInputHandler.Instance.ShowNewOptions += ShowNewOptions;
        SwapInputHandler.Instance.ShowConfirmScreen += ShowConfirmOptions;
        difficultySettings = DifficultyManager.Instance.difficultySettings;
    }

    void HideWindows()
    {
        if (optionsWindow.activeSelf)
        {
            optionsWindow.SetActive(false);
        }

        if (confirmWindow.activeSelf)
        {
            confirmWindow.SetActive(false);
        }
    }

    public void ShowNewOptions()
    {
        HideWindows();
        optionsWindow.SetActive(true);
        SoundController.Instance.PlaySoundEffect(windowOpeningAudio);
        // Check to make sure if we should offer the middle placement upgrade
        bool middleUpgraded = false;
        List<InventorySlot> slot = InventoryManager.Instance.GetUpgradeableLetters();
        if (slot.Count == 0) middleUpgraded = true;
        // God mode can only appear once as an option
        bool godModeUpgraded = false;

        // Wildcard can only appear once and only if there is over a certain amount of valid rows
        bool wildCardUpgraded = playArea.GetSysTileOnlyRows().Count >= difficultySettings.minSoloSysTileCount ? false : true;


        // Setup inventory to ensure that we are not giving player a letter they already have
        List<char> used = InventoryManager.Instance.GetLetterContents();
        List<char> data = new();
        List<OPTION_TYPE> upgradeType = new();

        for (int i = 0; i < optionsNumber; i++)
        {
            OPTION_TYPE type = GenerateOptionType(middleUpgraded, godModeUpgraded, wildCardUpgraded);
            if (type == OPTION_TYPE.SWAP)
            {
                char c = LetterUtility.GenerateUniqueLetter(used);
                used.Add(c);
                data.Add(c);
                upgradeType.Add(OPTION_TYPE.SWAP);
            }
            else if (type == OPTION_TYPE.MIDDLE_PLACEMENT)
            {
                // This is a placeholder char to represent the middle letter placement upgrade
                data.Add('-');
                upgradeType.Add(OPTION_TYPE.MIDDLE_PLACEMENT);
                middleUpgraded = true;
            }else if (type == OPTION_TYPE.GOD_MODE)
            {
                data.Add('*');
                upgradeType.Add(OPTION_TYPE.GOD_MODE);
                godModeUpgraded = true;
            }else if (type == OPTION_TYPE.WILD_CARD)
            {
                data.Add('?');
                upgradeType.Add(OPTION_TYPE.WILD_CARD);
                wildCardUpgraded = true;
            }
        }
        swapGenerator.PopulateOptions(data, upgradeType);

    }

    private OPTION_TYPE GenerateOptionType(bool middleUpgradeGenerated, bool godOptionGenerated, bool wildCardGenerated)
    {
        int wordsSubmitted = GameManager.Instance.CorrectWordsSubmitted;
        DifficultySettingsSO settings = DifficultyManager.Instance.difficultySettings;
        if (!middleUpgradeGenerated && wordsSubmitted >= settings.minMiddleAnsCount)
        {
            int rand = Random.Range(0, 101);
            if (rand <= difficultySettings.middleUpgradeChance)
            {
                return OPTION_TYPE.MIDDLE_PLACEMENT;
            }
        }
        if (!godOptionGenerated && wordsSubmitted >= settings.minGodModeAnsCount)
        {
            int rand = Random.Range(0, 101);
            if (rand <= difficultySettings.godModeUpgradeChance)
            {
                return OPTION_TYPE.GOD_MODE;
            }
        }
        if (!wildCardGenerated && wordsSubmitted >= settings.minSoloSysTileCount)
        {
            int rand = Random.Range(0, 101);
            if (rand <= difficultySettings.wildCardUpgradeChance)
            {
                return OPTION_TYPE.WILD_CARD;
            }
        }
        return OPTION_TYPE.SWAP;
    }

    public void ShowConfirmOptions(char c)
    {
        HideWindows();
        confirmWindow.SetActive(true);
        if(c == '-')
        {
            List<char> data = new();
            List<OPTION_TYPE> optionType = new();
            List<InventorySlot> slots = InventoryManager.Instance.GetUpgradeableLetters();
            for (int i = 0; i < slots.Count; i++)
            {
                data.Add(slots[i].Content[0]);
                optionType.Add(OPTION_TYPE.MIDDLE_UPGRADE_CONFIRM);
            }
            confirmGenerator.PopulateOptions(data, optionType);
            confirmGenerator.SetInstructions($"Choose the slot you wish to upgrade:");
        }
        else
        {
            List<char> inv;
            if (LetterUtility.IsVowel(c))
            {
                inv = InventoryManager.Instance.GetAllVowels();
            }
            else
            {
                inv = InventoryManager.Instance.GetAllConsonants();
            }
            List<char> data = new();
            List<OPTION_TYPE> optionType = new();
            for (int i = 0; i < inv.Count; i++)
            {
                data.Add(inv[i]);
                optionType.Add(OPTION_TYPE.SWAP_CONFIRM);
            }
            confirmGenerator.PopulateOptions(data, optionType);
            confirmGenerator.SetInstructions($"Swap {c} with:");
        }
        
    }

}
