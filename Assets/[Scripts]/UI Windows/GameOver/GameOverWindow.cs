using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverWindow : MonoBehaviour
{
    // Just the gameobject parent of all visible UI elements
    [SerializeField]
    GameObject visibleContainer;
    [SerializeField]
    TextMeshProUGUI tmpCorrectCount;
    [SerializeField]
    TextMeshProUGUI tmpIncorrectCount;
    [SerializeField]
    TextMeshProUGUI tmpPowerpack;

    // For temp debugging
    [SerializeField]
    TextMeshProUGUI tmp;

    // Start is called before the first frame update
    void Start()
    {
        if (visibleContainer.activeSelf)
        {
            visibleContainer.SetActive(false);
        }
        GameManager.Instance.GameStateChanged += OnStateChanged;
    }

    private void OnStateChanged(GameManager.GameState state)
    {
        if(state == GameManager.GameState.LOST)
        {
            visibleContainer.SetActive(true);
            tmp.text = GameManager.Instance.GameOverCause();
            tmpCorrectCount.text = $"Correct Count: {GameManager.Instance.CorrectWordsSubmitted}";
            tmpIncorrectCount.text = $"Incorrect Count: {GameManager.Instance.IncorrectWords}";
            tmpPowerpack.text = $"Powerups Used: {PowerupPack.PowerPackUsed}";
        }
        else
        {
            visibleContainer.SetActive(false);
        }
    }

}
