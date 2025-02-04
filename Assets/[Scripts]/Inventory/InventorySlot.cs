using System;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public bool MiddleLetter { get; private set; } = false;
    public string Content { get; private set; } = string.Empty;
    public Action ContentChanged;

    public void Initialize(string str, bool isMiddleLetter = false)
    {
        Empty();
        Content = str;
        MiddleLetter = isMiddleLetter;
        ContentChanged?.Invoke();
    }

    /// <summary>
    /// Clears the inventory slot and empties the contents
    /// </summary>
    public void Empty()
    {
        Content = string.Empty;
        MiddleLetter = false;
        ContentChanged?.Invoke();
    }

    public void ChangeLetter(string str)
    {
        Content = str;
        ContentChanged?.Invoke();
    }

    public void UpgradeMiddle()
    {
        MiddleLetter = true;
        ContentChanged?.Invoke();
    }

    public bool IsEmpty()
    {
        return Content == string.Empty;
    }
}
