using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class PauseToggle : MonoBehaviour, IUICollider, IPointerClickHandler
{
    [SerializeField]
    Sprite pauseIcon;
    [SerializeField]
    Sprite playIcon;
    [SerializeField]
    Image buttonIconImg;
    [SerializeField]
    Window pauseWindow;

    public event Action SizeChanged;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameStateChanged += OnStateChanged;
        OnStateChanged(GameManager.Instance.CurrentState);
        InvokeSizeChanged();
    }

    public void TogglePause()
    {
        if(GameManager.Instance.CurrentState == GameManager.GameState.IN_PLAY)
        {
            GameManager.Instance.TrySetGameState(GameManager.GameState.PAUSED);
        }
        else if(GameManager.Instance.CurrentState == GameManager.GameState.PAUSED)
        {
            GameManager.Instance.TrySetGameState(GameManager.GameState.IN_PLAY);
        }
        Display();
        
    }

    private void OnStateChanged(GameManager.GameState gameState)
    {
        Display();
        if (GameManager.Instance.CurrentState == GameManager.GameState.PAUSED)
        {
            pauseWindow.ShowWindow();
        }
        else
        {
            pauseWindow.CloseWindow();
        }
    }

    private void Display()
    {
        if(GameManager.Instance.CurrentState == GameManager.GameState.IN_PLAY)
        {
            buttonIconImg.sprite = pauseIcon;
        }
        else if(GameManager.Instance.CurrentState == GameManager.GameState.PAUSED)
        {
            buttonIconImg.sprite = playIcon;
        }
        InvokeSizeChanged();
    }

    public void InvokeSizeChanged()
    {
        SizeChanged?.Invoke();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TogglePause();
    }
}
