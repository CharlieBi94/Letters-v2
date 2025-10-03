using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;

public class PowerupPack : MonoBehaviour
{
    public static int PowerPackUsed = 0;
    [SerializeField]
    AudioClip openPackAudio;
    [SerializeField]
    TextMeshProUGUI tmp;
    [SerializeField]
    CatAnimator catAnimator;
    public int MAX_POWERPACK_COUNT;
    public int PowerPackCount { get; private set; }
    public Action<int> PowerPackCountChanged;
    public Action PowerPackEarned;
    [SerializeField]
    List<Image> powerPackImages;
    


    // Start is called before the first frame update
    void Start()
    {
        PowerPackCount = 0;
        UpdateDisplay();
        GameManager.Instance.BeginLevelup += OnLevelUp;
    }

    private void OnLevelUp()
    {
        if (PowerPackCount >= MAX_POWERPACK_COUNT) return;
        PowerPackCount++;
        PowerPackEarned?.Invoke();
        PowerPackCountChanged?.Invoke(PowerPackCount);
        catAnimator.CatSlapComplete += OnAnimComplete;
        
    }

    private void OnAnimComplete()
    {
        UpdateDisplay();
        catAnimator.CatSlapComplete -= OnAnimComplete;
    }

    public void OpenPowerPack()
    {
        if (PowerPackCount <= 0) return;
        if (GameManager.Instance.TrySetGameState(GameState.PAUSED) != GameState.PAUSED) return;
        SoundController.Instance.PlaySoundEffect(openPackAudio);
        PowerPackCount--;
        PowerPackUsed++;
        PowerPackCountChanged?.Invoke(PowerPackCount);
        UpdateDisplay();
        SwapInputHandler.Instance.ShowPowerPack();
        
    }

    private void UpdateDisplay()
    {
        tmp.text = PowerPackCount.ToString();
        for (int i = 0; i < powerPackImages.Count; i++)
        {
            if(i >= PowerPackCount)
            {
                powerPackImages[i].gameObject.SetActive(false);
            }
            else
            {
                powerPackImages[i].gameObject.SetActive(true);
            }
        }
    }
}
