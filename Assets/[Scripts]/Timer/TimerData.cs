using System;
using UnityEngine;

public class TimerData : MonoBehaviour
{
    [HideInInspector]
    public int minute = 0;
    [HideInInspector]
    public float second = 0;
    bool timerRunning = false;
    public Action TimeUp;
    public Action TimeChanged;

    // Update is called once per frame
    void Update()
    {
        if (timerRunning)
        {
            if(Time.deltaTime > second)
            {
                if (minute > 0)
                {
                    minute--;
                    second += 60;
                }
                else
                {
                    timerRunning = false;
                    TimeUp?.Invoke();
                }
            }
            second -= Time.deltaTime;
            TimeChanged?.Invoke();
        }
    }

    public void StartTimer(int min, float sec)
    {
        minute = min;
        second = sec;
        timerRunning = true;
    }

    /// <summary>
    /// Attempts to resume the timer countdown.
    /// </summary>
    /// <returns>True if successful</returns>
    public bool ResumeTimer()
    {
        if (ValidTimer())
        {
            timerRunning = true;
        }
        return timerRunning;
    }

    public void PauseTimer()
    {
        timerRunning = false;
    }

    private bool ValidTimer()
    {
        return (minute > 0 || second > 0);
    }

    public void AddTime(int minutes, float seconds)
    {
        minute += minutes;
        second += seconds;
        if(second > 60)
        {
            minute++;
            second = second % 60;

        }
    }

    public void RemoveTime(float sec)
    {
        if(sec > second)
        {
            if(minute > 0)
            {
                minute--;
                second += 60;
                
            }
            else
            {
                second = 0;
            }
        }
        second -= sec;
    }

    public bool IsTimeUp()
    {
        return !timerRunning;
    }

}
