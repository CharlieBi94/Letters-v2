using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Loads all sprites and provides them based on char value
/// </summary>
public class LetterSpriteLoader
{
    private static readonly string PATH = "LetterSprites";
    private static readonly Dictionary<char, Sprite> sprites = new();
    static LetterSpriteLoader()
    {
        LoadSprites();
    }

    public static bool IsInitialized()
    {
        return sprites.Count > 0;
    }

    private static void LoadSprites()
    {
        var letters = Resources.LoadAll(PATH, typeof(Sprite)).Cast<Sprite>().ToArray();
        for(int i = 0; i < 26; i++)
        {
            // convert the i into lowercase alphabet letters
            char c = (char)(i + 97);
            sprites.Add(c, letters[i]);
        }
    }

    // Returns the sprite that represents the char (case-insensitive)
    public static Sprite GetSprite(char c)
    {
        c = char.ToLower(c);
        if (sprites.ContainsKey(c))
        {
            return sprites[c];
        }
        else
        {
            return null;
        }
    }
}
