using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void HandleWildCardSelect()
    {
        //throw new NotImplementedException();
        // Look for eligible rows
        List<RowController> eligibleRows = SwapInputHandler.Instance.playArea.GetEmptyRows();
        foreach(RowController row in eligibleRows)
        {
            Tile t = row.GetAllTiles()[0];
            t.SetChar('?');
            t.gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            t.gameObject.GetComponent<Button>().onClick.AddListener(() => { SwapInputHandler.Instance.ShowWildCardOptions(t); });

        }
        SwapInputHandler.Instance.CompleteLevelUp();
    }
}
