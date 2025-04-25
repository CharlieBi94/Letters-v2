using System.Collections;
using System.Collections.Generic;

public class WordBank : Singleton<WordBank>
{
    List<string> words = new();

    public void AddWord(string word)
    {
        words.Add(word.ToLower());
    }

    public List<string> GetWordsSubmitted()
    {
        return words;
    }

    public bool Contains(string word)
    {
        return words.Contains(word.ToLower());
    }

    public void Reset()
    {
        words.Clear();
    }

}
