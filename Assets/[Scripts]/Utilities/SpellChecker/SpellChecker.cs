using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR;

/// <summary>
/// Class used for checking the spelling of words.
/// </summary>
public class SpellChecker
{
    // file path to the dictionary file (inside the Resources folder)
    private static readonly string FILE_PATH = "Dictionary/2of12inf";
    private static readonly HashSet<string> words = new();
    static SpellChecker()
    {
        PopulateDictionary();
    }
    /// <summary>
    /// Uses specified file to populate the internal dictionary
    /// </summary>
    public static void PopulateDictionary()
    {
        if(IsInitialized())
        {
            return;
        }
        TextAsset dictionaryFile = Resources.Load<TextAsset>(FILE_PATH);
        if (dictionaryFile == null)
        {
            Debug.Log($"Dictionary file not found.");
            return;
        }
        string[] lines = dictionaryFile.text.Split('\n');
        foreach (string line in lines)
        {
            char[] arr = line.Where(c => char.IsLetter(c)).ToArray();
            string s = new(arr);
            words.Add(s.ToLower());  // Store as lowercase for case-insensitive checking
        }
        
    }

    public static bool IsInitialized() { return words.Count != 0; }

    /// <summary>
    /// Checks to see if word provided is in the dictionary.
    /// </summary>
    /// <param name="s">case-insensitive</param>
    /// <returns></returns>
    public static bool SpellCheck(string s)
    {
        s = s.ToLower();
        return words.Contains(s);
    }
}
