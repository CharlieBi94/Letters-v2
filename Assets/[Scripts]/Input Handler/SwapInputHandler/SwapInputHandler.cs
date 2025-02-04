using System;
using UnityEngine;

public class SwapInputHandler : Singleton<SwapInputHandler>
{
    public Action ShowNewOptions;
    public Action<char> ShowConfirmScreen;

    [SerializeField]
    GameObject optionsScreen;

    [SerializeField]
    GameObject swapUI;

    [SerializeField]
    RectTransform visibilityButton;

    char newChar;

    char oldChar;

    // Start is called before the first frame update
    void Start()
    {
        Restart();
        // Subscribe to swap event from GameManager
        GameManager.Instance.BeginLevelup += OnBeginLevelup;
    }

    /// <summary>
    /// Used to toggle the swap UI (this is everything except the toggle visibility button)
    /// </summary>
    public void ToggleUI()
    {
        // Only do this if swapping has been initiated
        if (swapUI.activeSelf)
        {
            HideUI();
        }
        else
        {
            ShowUI();
        }
    }

    private void HideUI()
    {
        swapUI.SetActive(false);
        // hacky way to flip the icon 
        visibilityButton.localScale = new Vector3(visibilityButton.localScale.x, visibilityButton.localScale.y * -1, visibilityButton.localScale.z);
    }

    private void ShowUI()
    {
        swapUI.SetActive(true);
        visibilityButton.localScale = new Vector3(visibilityButton.localScale.x, visibilityButton.localScale.y * -1, visibilityButton.localScale.z);
    }

    private void OnBeginLevelup()
    {
        // first show UI
        if (!optionsScreen.activeSelf) 
        {
            optionsScreen.SetActive(true);
        }
        ShowNewOptions?.Invoke();

    }

    public void CompleteSwap()
    {
        // Check if both letters are populated
        if(!char.IsLetter(oldChar)) { return; }
        if (!char.IsLetter(newChar)) { return; }
        // Check if both characters are of the same type
        if(LetterUtility.IsVowel(oldChar) != LetterUtility.IsVowel(newChar)) { return; }
        InventoryManager.Instance.Swap(oldChar, newChar);
        GameManager.Instance.CompleteLevelup();
        Restart();
    }

    public void Restart()
    {
        ClearNewChar();
        ClearOldChar();
        if (optionsScreen.activeSelf)
        {
            optionsScreen.SetActive(false);
        }
    }

    public void SetNewLetter(char c)
    {
        newChar = c;
        ShowConfirmScreen?.Invoke(c);
    }

    public void SetOldLetter(char c)
    {
        oldChar = c;
    }

    public void SelectMiddleUpgrade()
    {
        // hacky way to introduce the middle letter upgrade
        ShowConfirmScreen?.Invoke('-');
    }

    public void CompleteMiddleUpgrade()
    {
        if (!char.IsLetter(oldChar)) { return; }
        InventoryManager.Instance.UpgradeMiddlePlacement(oldChar);
        GameManager.Instance.CompleteLevelup();
        Restart();
    }

    private void ClearOldChar()
    {
        oldChar = '\0';
    }

    private void ClearNewChar()
    {
        newChar = '\0';
    }

    
}
