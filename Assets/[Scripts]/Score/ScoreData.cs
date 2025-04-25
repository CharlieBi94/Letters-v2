using System;
using UnityEngine;

public class ScoreData : Singleton<ScoreData>
{
    float score;
    public Action ScoreChanged;


    private void Start()
    {
        score = 0;
    }

    public float GetScore()
    {
        return score;
    }

    public void ChangeScore(float amount)
    {
        score += amount;
        ScoreChanged?.Invoke();
    }
}
