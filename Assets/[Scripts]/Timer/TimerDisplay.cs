using TMPro;
using UnityEngine;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tmpDisplay;
    TimerData timerData;
    void Start()
    {
        timerData = GetComponent<TimerData>();
        timerData.TimeChanged += OnTimeChanged;
    }

    private void OnTimeChanged()
    {
        string formattedSec = string.Format("{0:00}", timerData.second);
        tmpDisplay.color = (timerData.minute == 0 && timerData.second < 30f) ? Color.red : Color.black;
        tmpDisplay.text = $"{timerData.minute}:{formattedSec}";
    }
}
