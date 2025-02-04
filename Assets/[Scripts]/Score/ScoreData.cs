using System;
using UnityEngine;

public class ScoreData : Singleton<ScoreData>
{
    int score;
    public Action ScoreChanged;


    private void Start()
    {
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public void ChangeScore(int amount)
    {
        score += amount;
        ScoreChanged?.Invoke();
    }
}
