using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

/// <summary>
/// Singleton responsible for managing a player's inventory
/// </summary>
public class InventoryManager : Singleton<InventoryManager>
{
    // Used to dynamically create inventory slots
    [SerializeField]
    GameObject inventorySlotPrefab;

    [SerializeField]
    RectTransform slotParent;

    // Data that holds the contents of the inventory
    private readonly Dictionary<string, InventorySlot> inventoryContents = new();
    private List<InventorySlot> slots;

    // Config options
    private const int MAX_SLOTS = 10;


    private void Start()
    {
        // Generate the maximum number slots
        for(int i = 0; i < MAX_SLOTS; i++)
        {
            GameObject obj = Instantiate(inventorySlotPrefab);
            obj.transform.SetParent(slotParent, false);
        }
        slots = slotParent.GetComponentsInChildren<InventorySlot>().ToList();
        FillInventoryRand();
    }

    private void FillInventoryRand()
    {
        int vowels = 0;
        List<char> vowelUsed = new();
        List<char> constUsed = new();
        InventorySlot inv = GetNextEmpty();
        while (inv != null)
        {
            char c;
            // generate 3 vowels first
            if (vowels < 3)
            {
                c = LetterUtility.GenerateUniqueVowel(vowelUsed);
                vowelUsed.Add(c);
                vowels++;
            }
            else
            {
                c = LetterUtility.GenerateUniqueConsonant(constUsed);
                constUsed.Add(c);
            }
            c = char.ToLower(c);
            inv.Initialize(c.ToString(), false);
            inventoryContents.Add(c.ToString().ToLower(), inv);
            inv = GetNextEmpty();
        }
    }

    /// <summary>
    /// Attempts to remove string / char from the inventory
    /// </summary>
    /// <param name="str"></param>
    /// <returns>True if successful, false if not</returns>
    public bool Remove(string str)
    {
        str = str.ToLower();
        if (!inventoryContents.ContainsKey(str))
        {
            Debug.LogError($"inventory does not contain {str}");
            return false; 
        }
        inventoryContents[str].Empty();
        inventoryContents.Remove(str);
        return true;
    }

    /// <summary>
    /// Attempts to add specified string/char to player inventory
    /// </summary>
    /// <param name="str"></param>
    /// <returns>True if successful, false if inventory is full</returns>
    public bool Add(string str)
    {
        str = str.ToLower();
        // Check to see if inventory is full
        InventorySlot inv = GetNextEmpty();
        if (inv == null)
        {
            Debug.LogError($"inventory full, cannot add {str}");
            return false; 
        }

        // Check to see if inventory already contains the item
        if (inventoryContents.ContainsKey(str)) {
            Debug.LogError($"inventory already contains {str}");
            return false; 
        }

        inv.Initialize(str);
        inventoryContents.Add(str, inv);
        return true;
    }

    /// <summary>
    /// Tries to find the closest avaliable empty inventory slot
    /// </summary>
    /// <returns>Null if unable to find an empty slot</returns>
    private InventorySlot GetNextEmpty()
    {
        for(int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty())
            {
                return slots[i];
            }
        }
        return null;
    }

    public List<char> GetLetterContents()
    {
        List<char> letters = new();
        inventoryContents.Keys.ToList().ForEach(x => { letters.Add(x[0]); });
        return letters;
    }

    public List<char> GetAllVowels()
    {
        List<char> letters = new();
        inventoryContents.Keys.ToList().ForEach(x => {
            if (LetterUtility.IsVowel(x[0]))
            {
                letters.Add(x[0]);
            }
        });
        return letters;
    }

    public List<char> GetAllConsonants()
    {
        List<char> letters = new();
        inventoryContents.Keys.ToList().ForEach(x => {
            if (!LetterUtility.IsVowel(x[0]))
            {
                letters.Add(x[0]);
            }
        });
        return letters;
    }

    public List<InventorySlot> GetUpgradeableLetters()
    {
        List<InventorySlot> letters = new();
        inventoryContents.Values.ToList().ForEach(x =>
        {
            if (x.MiddleLetter == false)
            {
                letters.Add(x);
            }

        });
        return letters;
    }

    public void UpgradeMiddlePlacement(char c)
    {
        string key = c.ToString().ToLower();
        if (inventoryContents.ContainsKey(key))
        {
            inventoryContents[key].UpgradeMiddle();
        }
    }

    /// <summary>
    /// Swaps the specified contents in the new slot with the old slot
    /// </summary>
    /// <param name="oldChar"></param>
    /// <param name="newChar"></param>
    public void Swap(char oldChar, char newChar)
    {
        
        string oldKey = oldChar.ToString().ToLower();
        if (!inventoryContents.ContainsKey(oldKey))
        {
            throw new System.Exception("Inventory manager tried to swap but didn't find specified contents");
        }
        InventorySlot slot = inventoryContents[oldKey];
        string newKey = newChar.ToString().ToLower();
        slot.ChangeLetter(newKey);
        inventoryContents.Remove(oldKey);
        inventoryContents.Add(newKey, slot);
    }
}
