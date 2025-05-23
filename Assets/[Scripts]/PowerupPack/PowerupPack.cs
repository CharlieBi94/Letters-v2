using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static GameManager;

public class PowerupPack : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI tmp;
    public int PowerPackCount { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        PowerPackCount = 0;
        tmp.text = PowerPackCount.ToString();
        GameManager.Instance.BeginLevelup += OnLevelUp;
    }

    private void OnLevelUp()
    {
        if (PowerPackCount > 0) return;
        PowerPackCount++;
        tmp.text = PowerPackCount.ToString();
    }

    public void OpenPowerPack()
    {
        if (GameManager.Instance.TrySetGameState(GameState.PAUSED) != GameState.PAUSED) return;
        PowerPackCount--;
        tmp.text = PowerPackCount.ToString();
        SwapInputHandler.Instance.ShowPowerPack();
    }
}
