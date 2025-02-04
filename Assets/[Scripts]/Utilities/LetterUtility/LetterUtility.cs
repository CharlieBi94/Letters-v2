using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Letter class used to compare and generate characters
/// </summary>
public static class LetterUtility
{
    private static readonly List<char> vowels = new() { 'a', 'e', 'i', 'o', 'u' };
    private static readonly List<char> consonants = new() { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' };
    
    public static char GenerateLetter()
    {
        return (char)Random.Range(65, 91);
    }
    public static char GenerateUniqueLetter(List<char> used)
    {
        List<char> upperUsed = new();
        used.ForEach(x => upperUsed.Add(char.ToUpper(x)));
        char c;
        do
        {
            c = (char)Random.Range(65, 91);
        }
        while (upperUsed.Contains(c));
        return c;
    }


    public static char GenerateUniqueVowel(List<char> used)
    {
        // convert the used list to lowercase
        List<char> possible = GetPossible(used, vowels);
        if(possible.Count == 0) { return '\0'; }
        return possible[Random.Range(0, possible.Count)];
    }

    public static char GenerateUniqueConsonant(List<char> used)
    {
        // convert the used list to lowercase
        List<char> possible = GetPossible(used, consonants);
        return possible[Random.Range(0, possible.Count)];
    }

    private static List<char> GetPossible(List<char> used, List<char> set)
    {
        List<char> possible = new(set);
        List<char> usedLower = new(used);
        usedLower.ForEach(x => 
        {
            char c = char.ToLower(x);
            if (possible.Contains(x))
            {
                possible.Remove(x);
            }
        });
        return possible;
    }

    public static bool IsVowel(char c)
    {
        c = char.ToLower(c);
        if (vowels.Contains(c))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
