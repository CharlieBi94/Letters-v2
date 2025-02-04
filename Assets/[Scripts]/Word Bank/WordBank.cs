using System.Collections;
using System.Collections.Generic;

public class WordBank : Singleton<WordBank>
{
    List<string> words = new();

    public void AddWord(string word)
    {
        words.Add(word);
    }

    public List<string> GetWordsSubmitted()
    {
        return words;
    }

    public void Reset()
    {
        words.Clear();
    }

}
