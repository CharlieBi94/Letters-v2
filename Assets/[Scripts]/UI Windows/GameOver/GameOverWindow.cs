using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverWindow : MonoBehaviour
{
    // Just the gameobject parent of all visible UI elements
    [SerializeField]
    GameObject visibleContainer;

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
            print("lost");
            visibleContainer.SetActive(true);
        }
        else
        {
            visibleContainer.SetActive(false);
        }
    }

}
