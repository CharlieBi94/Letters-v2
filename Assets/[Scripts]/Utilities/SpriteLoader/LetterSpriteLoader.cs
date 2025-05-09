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
        // Loads power up sprites
        Sprite godModeIcon = (Sprite)Resources.Load("PowerUps/god_mode", typeof(Sprite));
        sprites.Add('*', godModeIcon);
        Sprite wildcardIcon = (Sprite)Resources.Load("PowerUps/wildcard_swap", typeof(Sprite));
        sprites.Add('?', wildcardIcon);
        Sprite wildcardTile = (Sprite)Resources.Load("PowerUps/wildcard_blank", typeof(Sprite));
        sprites.Add('>', wildcardTile);
        Sprite middleUpgradeIcon = (Sprite)Resources.Load("PowerUps/middle", typeof(Sprite));
        sprites.Add('-', middleUpgradeIcon);
        // Loads number sprites
        var numbers = Resources.LoadAll("NumberSprites", typeof(Sprite)).Cast<Sprite>().ToArray();
        for(int i = 0; i < numbers.Length; i++)
        {
            sprites.Add((char)(48 + i + 1), numbers[i]);
        }
    }

    // Returns the sprite that represents the char (case-insensitive)
    public static Sprite GetSprite(char c, bool isTile = false)
    {
        c = char.ToLower(c);
        if (sprites.ContainsKey(c))
        {
            if (c == '?' && isTile) return sprites['>'];
            return sprites[c];
        }
        else
        {
            return null;
        }
    }
}
