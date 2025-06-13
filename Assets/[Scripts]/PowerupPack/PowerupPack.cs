using System;
using TMPro;
using UnityEngine;
using static GameManager;

public class PowerupPack : MonoBehaviour
{
    public static int PowerPackUsed = 0;

    [SerializeField]
    TextMeshProUGUI tmp;
    public int MAX_POWERPACK_COUNT;
    public int PowerPackCount { get; private set; }
    public Action<int> PowerPackCountChanged;

    // Start is called before the first frame update
    void Start()
    {
        PowerPackCount = 0;
        tmp.text = PowerPackCount.ToString();
        GameManager.Instance.BeginLevelup += OnLevelUp;
    }

    private void OnLevelUp()
    {
        if (PowerPackCount >= MAX_POWERPACK_COUNT) return;
        PowerPackCount++;
        tmp.text = PowerPackCount.ToString();
        PowerPackCountChanged?.Invoke(PowerPackCount);
    }

    public void OpenPowerPack()
    {
        if (PowerPackCount <= 0) return;
        if (GameManager.Instance.TrySetGameState(GameState.PAUSED) != GameState.PAUSED) return;
        PowerPackCount--;
        PowerPackUsed++;
        PowerPackCountChanged?.Invoke(PowerPackCount);
        tmp.text = PowerPackCount.ToString();
        SwapInputHandler.Instance.ShowPowerPack();
    }
}
