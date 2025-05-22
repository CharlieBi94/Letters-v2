using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WordBank : Singleton<WordBank>
{
    private readonly Dictionary<string, int> wordSubmitted = new Dictionary<string, int>();

    public void AddWord(string word)
    {
        word = word.ToLower();
        if(wordSubmitted.ContainsKey(word))
        {
            wordSubmitted[word]++;
        }
        else
        {
            wordSubmitted.Add(word, 1);
        }
    }

    public List<string> GetWordsSubmitted()
    {
        return wordSubmitted.Keys.ToList();
    }

    public bool Contains(string word)
    {
        return wordSubmitted.ContainsKey(word.ToLower());
    }

    public int GetWordCount(string word)
    {
        word = word.ToLower();
        if (wordSubmitted.ContainsKey(word))
        {
            return wordSubmitted[word];
        }
        else
        {
            return 0;
        }
    }

    public void Reset()
    {
        wordSubmitted.Clear();
    }

}
