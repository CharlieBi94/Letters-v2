using System;
using System.Collections;
using UnityEngine;

public class MovesTracker : MonoBehaviour
{
    [SerializeField]
    ProgressDisplay progressDisplay;
    // Start is called before the first frame update
    void Start()
    {
        progressDisplay.SetValue(0);
        StartCoroutine(Initialize());
    }

    private IEnumerator Initialize()
    {
        yield return new WaitUntil(() => GameManager.Instance != null);
        GameManager.Instance.LevelupMovesChanged += OnLevelupMovesChanged;
    }

    private void OnLevelupMovesChanged(int moves, int max)
    {
        if(moves == 0)
        {
            progressDisplay.SetValue(0);
        }
        else
        {
            int remainder = moves % max;
            float percent = (float)remainder / (float)max;
            if (remainder == 0)
            {
                progressDisplay.SetValue(1);
                progressDisplay.SetValue(0);
            }
            else
            {
                progressDisplay.SetValue(percent);
            }
            
            
        }
        
    }
}
