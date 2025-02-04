using System;
using UnityEngine;

[RequireComponent(typeof(OptionData))]
public class OptionBehaviour : MonoBehaviour
{
    OptionData data;
    private void Start()
    {
        data = GetComponent<OptionData>();
    }
    public void HandleSwap()
    {
        SwapInputHandler.Instance.SetNewLetter(data.content);
    }
    public void HandleConfirm()
    {
        SwapInputHandler.Instance.SetOldLetter(data.content);
        SwapInputHandler.Instance.CompleteSwap();
    }
    public void HandleMiddleUpgrade()
    {
        SwapInputHandler.Instance.SelectMiddleUpgrade();
    }
    public void HandleMiddleUpgradeConfirm()
    {
        SwapInputHandler.Instance.SetOldLetter(data.content);
        SwapInputHandler.Instance.CompleteMiddleUpgrade();
    }

    internal void HandleGodModeSelect()
    {
        GameManager.Instance.StartGodMode();
        SwapInputHandler.Instance.Restart();
    }
}
